using Chillgo.Api.Models.Request;
using Chillgo.Api.Models.Response;
using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin, Nhân Viên Quản Lý")]
        [HttpGet("statistical")]
        public async Task<IActionResult> GetAccountsStatistic()
        {
            return Ok(await _serviceFactory.GetAccountService().TotalAccount());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ViewAccount([FromRoute] Guid id)
        {
            var accountInfo = await _serviceFactory.GetAccountService().GetAccountByIdAsync(id);
            return Ok(accountInfo.Adapt<RS_AccountSecured>());
        }

        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetListAccount([FromQuery] RQ_AccountFilter queryFilter)
        {

            BM_PagingResults<BM_AccountBaseInfo> accList = await _serviceFactory.GetAccountService().GetAccountsListAsync
                (queryFilter.Adapt<BM_AccountQuery>());

            return Ok(accList);
        }

        //----------------------------------------------------------------------------

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RQ_AccountAuth clientAccount)
        {
            var result = await _serviceFactory.GetAccountService().CreateAccountAsync(clientAccount.Adapt<BM_Account>());
            return result ? Created(nameof(Register), "Tạo thành công!") : BadRequest("Tạo thất bại!");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RQ_AccountAuth clientAccount)
        {
            var result = await _serviceFactory.GetAccountService().LoginByPasswordAsync(clientAccount.Adapt<BM_Account>());

            if (result.accInfo is null || string.IsNullOrEmpty(result.jwtToken)) { return Unauthorized("Đã có lỗi khi đăng nhập! Vui lòng thử lại sau."); }

            return Ok(new
            {
                JwtToken = result.jwtToken,
                AccountInfo = result.accInfo
            });
        }

        /*[HttpPost("google-authentication")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleResponse responseData)
        {
            string jwtToken = await _serviceFactory.GetAccountService().HandleGoogleAsync(responseData.Token, responseData.Platform);

            return jwtToken.IsNullOrEmpty() ?
                BadRequest("There is an error during generate JWT Token!") :
                Created(nameof(Login), new JwtToken
                {
                    Token = jwtToken
                });
        }
        */

        //----------------------------------------------------------------------------
        [Authorize]
        [HttpPatch("updating")]
        public async Task<IActionResult> ChangeRole([FromBody] RQ_AccountBaseInfo updatedAccount)
        {
            bool result = await _serviceFactory.GetAccountService().UpdateAccountAsync(updatedAccount.Adapt<BM_Account>());
            
            return result == true ? Ok("Update Success!") : BadRequest("Update Failed!");
        }

        [Authorize]
        [HttpPatch("role-management")]
        public async Task<IActionResult> ChangeRole([FromBody] RQ_AccountPermission clientRequest)
        {
            bool result = await _serviceFactory.GetAccountService().ChangeRoleAccountAsync(clientRequest.Adapt<BM_Account>(), clientRequest.targetAccountId);

            return result == true ? Ok("Update Success!") : BadRequest("Update Failed!");
        }

        //[HttpPatch("password-recovery")]
        //public async Task<IActionResult> ResetPassword()
        //{
        //    //Is doing with Google Cloud Api
        //    return Ok();
        //}

        [Authorize]
        [HttpPatch("new-password")]
        public async Task<IActionResult> ChangePassword([FromBody] RQ_AccountPermission input)
        {
            if (string.IsNullOrEmpty(input.Password) || string.IsNullOrEmpty(input.NewPassword))
            { return BadRequest("Both type of password required for handle!"); }

            bool result = await _serviceFactory.GetAccountService().ChangePasswordAccountAsync(input.Adapt<BM_Account>(), input.targetAccountId, input.NewPassword);
            return result ? Ok("Update Success!") : BadRequest("Update Failed!");
        }

        //----------------------------------------------------------------------------
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] RQ_AccountPermission clientRequest)
        {
            bool result = await _serviceFactory.GetAccountService().DeleteAccountAsync(clientRequest.Adapt<BM_Account>(), clientRequest.targetAccountId);
            return result ? Ok("Delete Success!") : BadRequest("Delete Failed!");
        }

    }
}
