using Chillgo.Api.Models.Request;
using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Chillgo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        //=================================[ Declares ]================================
        private readonly IServiceFactory _serviceFactory;

        public AccountsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        //=================================[ Endpoints ]================================
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] AccountBaseInfo account)
        {
            BM_Account ServiceAcc = new BM_Account();
            var result = await _serviceFactory.GetAccountService().CreateAccountAsync(account.Adapt(ServiceAcc));
            return result ? Ok("Tạo thành công!") : BadRequest("Tạo thất bại!");
        }

        [HttpGet("Count")]
        public async Task<ActionResult> TestConnect()
        {
            return Ok(await _serviceFactory.GetAccountService().CountAccount());
        }
    }
}
