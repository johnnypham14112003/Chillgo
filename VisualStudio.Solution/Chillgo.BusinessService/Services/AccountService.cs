using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Extensions.Exceptions;
using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using Mapster;
using System.Security.Cryptography;
using System.Text;

using FirebaseAdmin.Auth;

namespace Chillgo.BusinessService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //=============================================================================
        public async Task<BM_AccountStatistics> TotalAccount()
        {
            return new BM_AccountStatistics
            {
                SumAccount = await _unitOfWork.GetAccountRepository().SummaryTotalAccount("", false),
                SumCustomer = await _unitOfWork.GetAccountRepository().SummaryTotalAccount("Người Dùng", true),
                SumTourGuide = await _unitOfWork.GetAccountRepository().SummaryTotalAccount("Hướng Dẫn Viên", true),
                SumPartner = await _unitOfWork.GetAccountRepository().SummaryTotalAccount("Đối Tác", true),
                SumStaff = await _unitOfWork.GetAccountRepository().SummaryTotalAccount("Nhân Viên Quản Lý", true),
                SumDeleted = await _unitOfWork.GetAccountRepository().SummaryTotalAccount("Đã Xóa", false)
            };
        }

        public async Task<BM_Account> GetAccountByIdAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
            {
                throw new BadRequestException("None account id like that");
            }
            var account = await _unitOfWork.GetAccountRepository().GetByIdAsync(accountId) ?? throw new NotFoundException("Not found that account id");

            var bm_account = account.Adapt<BM_Account>();

            return bm_account;
        }

        public async Task<BM_PagingResults<BM_AccountBaseInfo>> GetAccountsListAsync(BM_AccountQuery queryCondition)
        {
            //Reset invalid number
            queryCondition.PageIndex = (queryCondition.PageIndex <= 0) ? 1 : queryCondition.PageIndex;
            queryCondition.PageSize = (queryCondition.PageSize <= 0) ? 10 : queryCondition.PageSize;

            var (result, totalCount) = await _unitOfWork.GetAccountRepository().GetAccountsListAsync
                (queryCondition.KeyWord,
                queryCondition.Gender,
                queryCondition.Role,
                queryCondition.Status,
                queryCondition.PageIndex,
                queryCondition.PageSize,
                queryCondition.NameDescendingOrder);

            if (totalCount == 0) { throw new NotFoundException("Not found any account"); }

            // Convert to return data type
            var mappedResult = result.Adapt<List<BM_AccountBaseInfo>>();

            return new BM_PagingResults<BM_AccountBaseInfo>
            {
                PageSize = queryCondition.PageSize,
                CurrentPage = queryCondition.PageIndex,
                TotalCount = totalCount,
                DataList = mappedResult
            };
        }

        public async Task<bool> CreateAccountAsync(BM_Account AccountFromClient)
        {
            try
            {
                if (string.IsNullOrEmpty(AccountFromClient.FullName))
                {
                    throw new BadRequestException("Tên người dùng rỗng!");
                }

                var existAcc = await _unitOfWork.GetAccountRepository().GetOneAsync(acc => acc.Email.ToLower().Equals(AccountFromClient.Email.ToLower()), false);
                if (existAcc is not null)
                {
                    throw new ConflictException("Tài khoản Email này đã tồn tại trong hệ thống");
                }

                //Hash Password
                string securedPassword = HashStringSHA256(AccountFromClient.Password);
                AccountFromClient.Password = securedPassword;

                //Map data of 'AccountFromClient'  (BM_Account -> Account)
                var AccountInDb = AccountFromClient.Adapt<Account>();

                //Create and get FirebaseUid
                AccountInDb.FirebaseUid = await FirebaseRegisterAccount(AccountFromClient); ;

                //Save to DB
                await _unitOfWork.GetAccountRepository().AddAsync(AccountInDb);
                bool saveRusult = await _unitOfWork.GetAccountRepository().SaveChangeAsync();

                //Rollback Firebase user creation if save in DB failed!
                if (saveRusult == false) await FirebaseAuth.DefaultInstance.DeleteUserAsync(AccountInDb.FirebaseUid);

                return saveRusult;
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Đã có lỗi ở hàm CreateAccountAsync: " + ex.Message);
            }
        }

        public async Task<(string firebaseToken, BM_AccountBaseInfo accInfo)> LoginByPasswordAsync(string email, string password)
        {
            try
            {
                // Authenticate with Firebase
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);

                // Check account in database
                var existAccount = await _unitOfWork.GetAccountRepository().GetOneAsync(acc => acc.Email.ToLower().Equals(email.ToLower()), false);
                if (existAccount is null)
                {
                    throw new UnauthorizedAccessException("Tài khoản không tồn tại trong hệ thống.");
                }

                if (!HashStringSHA256(password).Equals(existAccount.Password))
                {
                    throw new BadRequestException("Sai mật khẩu đăng nhập!");
                }

                // Create Firebase Jwt token
                string jwtToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(userRecord.Uid);

                BM_AccountBaseInfo accountInfo = existAccount.Adapt<BM_AccountBaseInfo>();

                return (jwtToken, accountInfo);
            }
            catch (FirebaseAuthException firebaseEx)
            {
                throw new BadRequestException($"Firebase authentication failed: {firebaseEx.Message}");
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Login failed: {ex.Message}");
            }
        }

        //=============================================================================================
        private string HashStringSHA256(string input)//SHA-256 Algorithm (1 way)
        {
            using SHA256 sha256Hasher = SHA256.Create();
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        private static void BannedChecker(string status)
        {
            if (status.ToLower().Equals("Bị Cấm".ToLower()))
            {
                throw new BadRequestException("Tài khoản Email này đã bị cấm truy vào hệ thống.");
            }
        }
        private async Task<string> FirebaseRegisterAccount(BM_Account newAccount)
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

                //string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(userRecord.Uid);

                return userRecord.Uid;
            }
            catch (FirebaseAuthException firebaseEx)
            {
                throw new BadRequestException("Đã có lỗi ở hàm FirebaseRegisterAccount: " + firebaseEx.Message);
            }
        }

        public Task<string> HandleGoogleAsync(string token, string platform)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangeRoleAccountAsync(Guid accountId, string newRole)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangePasswordAccountAsync(Guid accountId, string oldPassword, string newPassword, bool privilegedOverride)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RecoverAccountAsync(string email, string newStatus)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAccountAsync(Guid accountId, string confirmPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> BanAccount(Guid accountId)
        {
            throw new NotImplementedException();
        }
    }
}
