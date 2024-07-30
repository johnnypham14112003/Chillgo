using Chillgo.Repository.Models;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IAccountService
    {
        Task<Account?> GetAccountsById(Guid id);
    }
}
