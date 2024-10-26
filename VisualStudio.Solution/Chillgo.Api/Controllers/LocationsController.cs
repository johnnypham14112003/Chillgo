using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Chillgo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;
        public LocationsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        // API lấy 5 địa điểm nổi nhất
        [Authorize]
        [HttpGet("Top5")]
        public ActionResult<List<Location>> GetTop5Locations()
        {
            var locations = _serviceFactory.GetLocationService().GetTop5Locations();
            return Ok(locations);
        }

        // API lấy 5 địa điểm ngẫu nhiên
        [Authorize]
        [HttpGet("Random5Location")]
        public ActionResult<List<Location>> GetRandom5Locations()
        {
            var locations = _serviceFactory.GetLocationService().GetRandom5Locations();
            return Ok(locations);
        }

        // API lấy danh sách đã sort và phân trang
        [Authorize]
        [HttpGet("SortedLocations")]
        public ActionResult<List<Location>> GetSortedLocations(string sortColumn = "Name", int page = 1, int pageSize = 10)
        {
            var locations = _serviceFactory.GetLocationService().GetSortedLocations(sortColumn, page, pageSize);
            return Ok(locations);
        }

        // API lấy thông tin chi tiết của 1 địa điểm
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Location> GetLocationById(Guid id)
        {
            var location = _serviceFactory.GetLocationService().GetLocationById(id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        [Authorize]
        [HttpGet("AllLocations")]
        public ActionResult<List<Location>> GetAllLocations()
        {
            var locations = _serviceFactory.GetLocationService().GetAllLocations();
            return Ok(locations);
        }
    }
}