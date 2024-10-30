using Chillgo.Api.Models.Request;
using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chillgo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : Controller
    {
        //=================================[ Declares ]================================
        private readonly IServiceFactory _serviceFactory;

        public PackageController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetListPackage([FromQuery] RQ_PackageFilter queryFilter)
        {
            var packList = await _serviceFactory.GetPackageService().GetListPackage
                (queryFilter.Adapt<BM_PackageQuery>());

            return Ok(packList);
        }
    }
}
