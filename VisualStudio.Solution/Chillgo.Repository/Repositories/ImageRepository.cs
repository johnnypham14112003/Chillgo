using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Chillgo.Repository.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly ChillgoDbContext _context;
        public ImageRepository(ChillgoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Image?> GetImageAsync(string fileName, byte typeReference)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            try
            {
                var query = _context.Images
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable();

                query = query.Where(img => img.UrlPath.ToLower().Contains(fileName.ToLower()));

                switch (typeReference)
                {
                    case 1: //'Account'
                        query = query.Where(img =>
                            img.AccountId.HasValue && img.AccountId.ToString().Equals(fileName));
                        break;

                    case 3: // 'Location'
                        query = query.Where(img =>
                            img.LocationId.HasValue && img.LocationId.ToString().Equals(fileName));
                        break;

                    default:
                        return null;
                }

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
