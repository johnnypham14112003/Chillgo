using Chillgo.Repository.Models;

namespace Chillgo.Repository.Interfaces
{
    public interface ILocationRepository
    {
        List<Location> GetTop5Locations();
        List<Location> GetRandom5Locations();
        List<Location> GetSortedLocations(string sortColumn, int page, int pageSize);

        Location GetLocationById(Guid id);
        List<Location> GetAllLocations();
    }
}
