using Chillgo.BusinessService.BusinessModels;
using Chillgo.Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IAccountService
    {
        Task<BM_AccountStatistics> TotalAccount();
        Task<BM_Account> GetAccountByIdAsync(Guid accountId);
        Task<BM_PagingResults<BM_AccountBaseInfo>> GetAccountsListAsync(BM_AccountQuery queryCondition);
        Task<bool> CreateAccountAsync(BM_Account newAccount);
        Task<(string firebaseToken, BM_AccountBaseInfo accInfo)> LoginByPasswordAsync(string email, string password);
        Task<string> HandleGoogleAsync(string token, string platform);
        Task<bool> ChangeRoleAccountAsync(Guid accountId, string newRole);
        Task<bool> ChangePasswordAccountAsync(Guid accountId, string oldPassword, string newPassword, bool privilegedOverride);
        Task<bool> RecoverAccountAsync(string email, string newStatus);
        Task<bool> DeleteAccountAsync(Guid accountId, string confirmPassword);
        Task<bool> BanAccount(Guid accountId);
    }
}
