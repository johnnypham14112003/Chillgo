using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.Repository.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ChillgoDbContext _dbContext;

        public LocationRepository(ChillgoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Lấy 5 địa điểm có đánh giá cao nhất
        public List<Location> GetTop5Locations()
        {
            return _dbContext.Locations
                .OrderByDescending(x => x.TotalRating)
                .Take(5)
                .ToList();
        }

        // Lấy 5 địa điểm ngẫu nhiên
        public List<Location> GetRandom5Locations()
        {
            return _dbContext.Locations
                .OrderBy(x => Guid.NewGuid())
                .Take(5)
                .ToList();
        }

        // Lấy danh sách địa điểm có sắp xếp và phân trang
        public List<Location> GetSortedLocations(string sortColumn, int page, int pageSize)
        {
            var query = _dbContext.Locations.AsQueryable();

            switch (sortColumn.ToLower())
            {
                case "totalrating":
                    query = query.OrderByDescending(x => x.TotalRating);
                    break;
                case "name":
                default:
                    query = query.OrderBy(x => x.Name);
                    break;
            }

            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
