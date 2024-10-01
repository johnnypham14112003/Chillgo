using Chillgo.BusinessService.BusinessModels;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> GetForCredentialsAsync(BM_Account newAccount);

    }
}
