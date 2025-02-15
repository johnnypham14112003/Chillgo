﻿using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;

namespace Chillgo.Repository.Repositories
{
    public class LocationRepository : GenericRepository<Location>,ILocationRepository
    {
        private readonly ChillgoDbContext _dbContext;

        public LocationRepository(ChillgoDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        // Lấy 5 địa điểm có đánh giá cao nhất
        public List<Location> GetTop5Locations()
        {
            TestDatabaseConnection();
            return _dbContext.Locations
                .OrderByDescending(x => x.TotalRating)
                .Take(5)
                .ToList();
        }
        

        public bool TestDatabaseConnection()
        {
            try
            {
                // Just trying to access the Locations table to see if the connection works
                return _dbContext.Locations.Any();
            }
            catch (Exception ex)
            {
                // Log or return error message for further analysis
                Console.WriteLine($"Connection test failed: {ex.Message}");
                return false;
            }
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


        // Lấy thông tin địa điểm theo ID
        public Location GetLocationById(Guid id)
        {
            return _dbContext.Locations.FirstOrDefault(x => x.Id == id);
        }


        public List<Location> GetAllLocations()
        {
            return _dbContext.Locations.ToList();
        }



    }
}
