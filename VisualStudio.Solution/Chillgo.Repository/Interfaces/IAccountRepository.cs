using Chillgo.Repository.Models;

namespace Chillgo.Repository.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        //Task<Account?> GetAccountDetail(Guid id);
        Task<int> SummaryTotalAccount(string whichType, bool byRole);
        Task<(List<Account> result, int totalCount)> GetAccountsListAsync
            (string? keyword, string? gender, string? role, string? status,
            int pageIndex, int pageSize, bool nameDescendingOrder);
    }
}
