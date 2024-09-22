using Chillgo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.Repository.Interfaces
{
    public interface ILocationRepository
    {
        List<Location> GetTop5Locations();
        List<Location> GetRandom5Locations();
        List<Location> GetSortedLocations(string sortColumn, int page, int pageSize);
    }
}
