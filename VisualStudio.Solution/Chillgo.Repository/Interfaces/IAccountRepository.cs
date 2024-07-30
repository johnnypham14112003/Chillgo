using Chillgo.Repository.Models;

namespace Chillgo.Repository.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        //Task<Account?> GetAccountDetail(Guid id);
    }
}
