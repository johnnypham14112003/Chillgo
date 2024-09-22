using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public List<Location> GetTop5Locations()
        {
            return _locationRepository.GetTop5Locations();
        }

        public List<Location> GetRandom5Locations()
        {
            return _locationRepository.GetRandom5Locations();
        }

        public List<Location> GetSortedLocations(string sortColumn, int page, int pageSize)
        {
            return _locationRepository.GetSortedLocations(sortColumn, page, pageSize);
        }
    }
}
