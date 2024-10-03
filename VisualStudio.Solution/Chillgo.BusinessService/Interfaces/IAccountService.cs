using Chillgo.BusinessService.BusinessModels;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IAccountService
    {
        Task<BM_AccountStatistics> TotalAccount();
        Task<BM_Account> GetAccountByIdAsync(Guid accountId);
        Task<BM_PagingResults<BM_AccountBaseInfo>> GetAccountsListAsync(BM_AccountQuery queryCondition);
        Task<bool> CreateAccountAsync(BM_Account newAccount);
        Task<(string jwtToken, BM_AccountBaseInfo accInfo)> LoginByPasswordAsync(BM_Account account);
        Task<string> HandleGoogleAsync(string token, string platform);
        Task<bool> UpdateAccountAsync(BM_Account updateAccount);
        Task<bool> ChangeRoleAccountAsync(BM_Account clientRequest, Guid targetAid);
        Task<bool> ChangePasswordAccountAsync(BM_Account clientRequest, Guid targetAid, string newPassword);
        Task<bool> DeleteAccountAsync(BM_Account clientRequest, Guid targetAid);
        Task<bool> BanAccount(Guid accountId);
    }
}
