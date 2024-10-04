using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Chillgo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        // API lấy 5 địa điểm nổi nhất
        [HttpGet("top-5")]
        public ActionResult<List<Location>> GetTop5Locations()
        {
            var locations = _locationService.GetTop5Locations();
            return Ok(locations);
        }

        // API lấy 5 địa điểm ngẫu nhiên
        [HttpGet("random-5-location")]
        public ActionResult<List<Location>> GetRandom5Locations()
        {
            var locations = _locationService.GetRandom5Locations();
            return Ok(locations);
        }

        // API lấy danh sách đã sort và phân trang
        [HttpGet("sorted-locations")]
        public ActionResult<List<Location>> GetSortedLocations(string sortColumn = "Name", int page = 1, int pageSize = 10)
        {
            var locations = _locationService.GetSortedLocations(sortColumn, page, pageSize);
            return Ok(locations);
        }
    }
}
