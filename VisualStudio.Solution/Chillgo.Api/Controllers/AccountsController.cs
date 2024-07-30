using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Models;
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
        [HttpGet("{accountId:Guid}")]
        public async Task<ActionResult<Account>> GetPagedProducts([FromRoute] Guid accountId)
        {
            return await _serviceFactory.GetAccountService().GetAccountsById(accountId);
        }
    }
}
