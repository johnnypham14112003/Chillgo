using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;

namespace Chillgo.BusinessService.Services
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Location> GetTop5Locations()
        {
            return _unitOfWork.GetLocationRepository().GetTop5Locations();
        }
        public List<Location> GetRandom5Locations()
        {
            return _unitOfWork.GetLocationRepository().GetRandom5Locations();
        }

        public List<Location> GetSortedLocations(string sortColumn, int page, int pageSize)
        {
            return _unitOfWork.GetLocationRepository().GetSortedLocations(sortColumn, page, pageSize);
        }

        public Location GetLocationById(Guid id)
        {
            return _unitOfWork.GetLocationRepository().GetLocationById(id);
        }

        public List<Location> GetAllLocations()
        {
            return _unitOfWork.GetLocationRepository().GetAllLocations();
        }


    }
}
