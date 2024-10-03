using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chillgo.BusinessService.Extensions.Exceptions
{
    public class FirebaseAuthorizeAttribute : TypeFilterAttribute
    {
        public FirebaseAuthorizeAttribute() : base(typeof(FirebaseAuthorizeFilter))
        {
        }
    }

    public class FirebaseAuthorizeFilter : IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            string authorizationHeader = context.HttpContext.Request.Headers["Authorization"]!;

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string token = authorizationHeader.Substring("Bearer ".Length);

            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                context.HttpContext.Items["FirebaseUserId"] = decodedToken.Uid;
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
