using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Interfaces;
using System.Net.Http.Json;

//Reference
//https://firebase.google.com/docs/reference/rest/auth#section-sign-in-email-password
//https://www.youtube.com/watch?v=xBuLWaDcvu0
namespace Chillgo.BusinessService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetForCredentialsAsync(BM_Account newAccount)
        {
            var request = new
            {
                newAccount.Email,
                newAccount.Password,
                returnSecureToken = true
            };

            // Need register to service and package Microsoft.Extensions.Http
            // Post custom user Id of firebase to get
            var response = await _httpClient.PostAsJsonAsync("", request);

            var authToken = await response.Content.ReadFromJsonAsync<BM_AuthToken>();

            return authToken.IdToken;
        }
    }
}
