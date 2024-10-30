using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Chillgo.Repository.Repositories
{
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        private readonly ChillgoDbContext _context;
        public PackageRepository(ChillgoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<Package>? result, int totalCount)> GetPackagesListAsync
            (string? keyword, decimal? price, short? duration, string? status, int pageIndex, int pageSize, bool nameDescendingOrder)
        {
            try
            {
                var query = _context.Packages
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable();

                // Apply search
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(pack =>
                    (pack.Code.ToLower().Contains(keyword.ToLower())) ||
                    (pack.Name.ToLower().Contains(keyword.ToLower())) ||
                    (pack.Description.ToLower().Contains(keyword.ToLower()))
                    );
                }

                if (price != null)
                {
                    query = query.Where(pack => pack.Price == price);
                }

                if (duration != null)
                {
                    query = query.Where(pack => pack.Duration == duration);
                }

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(pack => pack.Status.ToLower().Equals(status!.ToLower()));
                }

                // Sort by Name
                query = nameDescendingOrder == true ?
                    query.OrderByDescending(pack => pack.Name) : query.OrderBy(pack => pack.Name);

                int count = query.Count();

                // Apply paging
                var pagedPackages = await query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (pagedPackages, count);
            }
            catch (Exception)
            {
                return (null, 0);
            }
        }
    }
}
