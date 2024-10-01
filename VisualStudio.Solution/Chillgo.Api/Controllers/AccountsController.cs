using Chillgo.Api.Models.Request;
using Chillgo.Api.Models.Response;
using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        [Authorize]
        [HttpGet("statistical")]
        public async Task<IActionResult> TestConnect()
        {
            return Ok(await _serviceFactory.GetAccountService().TotalAccount());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ViewAccount([FromRoute] Guid id)
        {
            return Ok(await _serviceFactory.GetAccountService().GetAccountByIdAsync(id));
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
            //Pre-validate email format
            if (!RQ_AccountAuth.IsValidEmail(clientAccount.Email)) { return BadRequest("Email không hợp lệ!"); }

            var result = await _serviceFactory.GetAccountService().CreateAccountAsync(clientAccount.Adapt<BM_Account>());
            return result ? Created() : BadRequest("Tạo thất bại!");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RQ_AccountAuth clientAccount)
        {
            // Validate the email address
            if (!RQ_AccountAuth.IsValidEmail(clientAccount.Email))
            {
                return BadRequest("Invalid email address!");
            }

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

        //----------------------------------------------------------------------------
        [HttpPatch("role-management")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeRole([FromBody] AccountNewRole input)
        {
            bool result = await _serviceFactory.GetAccountService().ChangeRoleAccountAsync(input.Id, input.NewRole);
            if (result == true)
            {
                return Ok("Update Success!");
            }
            return BadRequest("Update Failed!");
        }

        [HttpPatch("password-recovery")]
        public async Task<IActionResult> ResetPassword()
        {
            //Is doing with Google Cloud Api
            return Ok();
        }

        [HttpPatch("new-password")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> ChangePassword([FromBody] AccountNewPassword input)
        {
            if (input.OldPassword.IsNullOrEmpty())
            {
                return BadRequest("Old password required for confirmation!");
            }
            bool result = await _serviceFactory.GetAccountService().ChangePasswordAccountAsync(input.Id, input.OldPassword, input.NewPassword, false);
            return result ? Ok("Update Success!") : BadRequest("Update Failed!");
        }

        //----------------------------------------------------------------------------
        [HttpDelete]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> Delete([FromBody] AccountConfirm acc)
        {
            bool result = await _serviceFactory.GetAccountService().DeleteAccountAsync(acc.Id, acc.Password);
            return result ? Ok("Delete Success!") : BadRequest("Delete Failed!");
        }*/

    }
}
