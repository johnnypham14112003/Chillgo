using Chillgo.BusinessService.BusinessModels;
using Chillgo.Repository.Models;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> FirebaseRegisterAccount(BM_Account newAccount);
        Task<string> CreateFirebaseCustomTokenAsync(Account account);
        Task<string> FetchForJwtToken(string CustomToken);

    }
}
