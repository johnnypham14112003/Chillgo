using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Extensions.Exceptions;
using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Models;
using FirebaseAdmin.Auth;
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
        public async Task<string> FirebaseRegisterAccount(BM_Account newAccount)
        {
            try
            {
                UserRecordArgs firebaseUser = new UserRecordArgs
                {
                    Email = newAccount.Email,
                    Password = newAccount.Password,
                    DisplayName = newAccount.FullName
                };

                UserRecord userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(firebaseUser);

                return userRecord.Uid;
            }
            catch (FirebaseAuthException firebaseEx)
            {
                throw new BadRequestException("Đã có lỗi ở hàm FirebaseRegisterAccount: " + firebaseEx.Message);
            }
        }

        public async Task<string> CreateFirebaseCustomTokenAsync(Account account)
        {
            var claims = new Dictionary<string, object>
        {
            { "role", account.Role },
            // Add any other custom claims you need
        };

            return await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(account.FirebaseUid, claims);
        }

        public async Task<string> FetchForJwtToken(string CustomToken)
        {
            var request = new
            {
                token = CustomToken,
                returnSecureToken = true
            };

            // Need register to service and package Microsoft.Extensions.Http
            // Post custom user Id of firebase to get jwt
            //The uri to fetch is config in ServiceRegistration
            var response = await _httpClient.PostAsJsonAsync("", request);

            var authToken = await response.Content.ReadFromJsonAsync<BM_AuthToken>();

            return authToken.IdToken;
        }
    }
}
