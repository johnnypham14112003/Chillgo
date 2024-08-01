using Chillgo.BusinessService.BusinessModels;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IAccountService
    {
        Task<bool> CreateAccountAsync(BM_Account newAccount);
        Task<int> CountAccount();
    }
}
