using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Extensions.Exceptions;
using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using Mapster;
using System.Security.Cryptography;
using System.Text;

using FirebaseAdmin.Auth;
using Microsoft.Identity.Client;

namespace Chillgo.BusinessService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;

        public AccountService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }

        //=============================================================================
        public async Task<BM_AccountStatistics> TotalAccount()
        {
            return new BM_AccountStatistics
            {
                SumAccount = await _unitOfWork.GetAccountRepository().SummaryTotalAccount("", false),
                SumCustomer = await _unitOfWork.GetAccountRepository().SummaryTotalAccount("Người Dùng", true),
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
            var (result, totalCount) = await _unitOfWork.GetAccountRepository().GetAccountsListAsync
                (queryCondition.KeyWord,
                queryCondition.Gender,
                queryCondition.Role,
                queryCondition.Status);

            if (totalCount == 0) { throw new NotFoundException("Not found any account"); }

            // Convert to return data type
            var mappedResult = result.Adapt<List<BM_AccountBaseInfo>>();

            return new BM_PagingResults<BM_AccountBaseInfo>
            {
                TotalCount = totalCount,
                DataList = mappedResult
            };
        }

        public async Task<bool> CreateAccountAsync(BM_Account AccountFromClient)
        {
            bool saveResult = false;
            bool isFirebaseTriggered = false;
            string firebaseUid = "";

            try
            {
                //Validate fullname
                if (string.IsNullOrEmpty(AccountFromClient.FullName)) throw new BadRequestException("Tên người dùng rỗng!");

                CheckValidEmail(AccountFromClient.Email);

                //Validate Password minimum lenght
                if (AccountFromClient.Password.Count() < 6)
                { throw new BadRequestException("Mật khẩu tối thiểu 6 ký tự!"); }

                var existAcc = await _unitOfWork.GetAccountRepository().GetOneAsync(acc =>
                acc.Email.ToLower().Equals(AccountFromClient.Email.ToLower()));

                // Validate exist - No deleted
                if (existAcc is not null && !DeletedChecker(existAcc.Status))
                { throw new ConflictException("Tài khoản Email này đã tồn tại trong hệ thống"); }

                //Hash Password
                string securedPassword = HashStringSHA256(AccountFromClient.Password);
                AccountFromClient.Password = securedPassword;

                //Register account to Firebase and get FirebaseUid
                firebaseUid = await _authenticationService.FirebaseRegisterAccount(AccountFromClient);
                isFirebaseTriggered = true;

                // if exist by deleted => recorver
                if (existAcc is not null && DeletedChecker(existAcc.Status))
                {
                    existAcc.Status = "Đã Xác Thực";
                    existAcc.FirebaseUid = firebaseUid;

                    saveResult = await _unitOfWork.GetAccountRepository().SaveChangeAsync();

                    //Rollback Firebase creation if save in DB failed!
                    if (saveResult == false && isFirebaseTriggered) await FirebaseAuth.DefaultInstance.DeleteUserAsync(firebaseUid);

                    return saveResult;
                }

                //Map data of 'AccountFromClient'  (BM_Account -> Account)
                var AccountInDb = AccountFromClient.Adapt<Account>();

                //Assign FirebaseUid
                AccountInDb.FirebaseUid = firebaseUid;

                //Save to DB
                await _unitOfWork.GetAccountRepository().AddAsync(AccountInDb);
                saveResult = await _unitOfWork.GetAccountRepository().SaveChangeAsync();

                return saveResult;
            }
            catch (Exception ex)
            {
                //Rollback Firebase user creation if save in DB failed!
                if (saveResult == false && isFirebaseTriggered) await FirebaseAuth.DefaultInstance.DeleteUserAsync(firebaseUid);
                //await FirebaseAuth.DefaultInstance.DeleteUserAsync(firebaseUid);
                throw new BadRequestException("Đã có lỗi ở hàm CreateAccountAsync: " + ex.Message);
            }
        }

        public async Task<(string jwtToken, BM_AccountBaseInfo accInfo)> LoginByPasswordAsync(BM_Account clientRequest)
        {
            try
            {
                CheckValidEmail(clientRequest.Email);

                // Check account in database
                var existAccount = await _unitOfWork.GetAccountRepository()
                    .GetOneAsync(acc => acc.Email.ToLower().Equals(clientRequest.Email.ToLower()), false);

                if (existAccount is null)
                { throw new UnauthorizedException("Tài khoản không tồn tại trong hệ thống."); }

                //Hash Password
                var securedPassword = HashStringSHA256(clientRequest.Password);

                // If password not match
                if (!securedPassword.Equals(existAccount.Password))
                { throw new BadRequestException("Sai mật khẩu đăng nhập!"); }

                // Authenticate with Firebase
                var firebaseClaims = await _authenticationService.CreateFirebaseCustomTokenAsync(existAccount);
                var firebaseJwtToken = await _authenticationService.FetchForJwtToken(firebaseClaims);

                //Query image avatar
                var ImageObj = await _unitOfWork.GetImageRepository().GetOneAsync(img => img.AccountId == existAccount.Id);

                var responseAccount = existAccount.Adapt<BM_AccountBaseInfo>();
                responseAccount.AvatarUrl = ImageObj is null ? "" : ImageObj.UrlPath;

                return (firebaseJwtToken, responseAccount);
            }
            catch (FirebaseAuthException firebaseEx)
            { throw new BadRequestException($"Firebase authentication failed: {firebaseEx.Message}"); }
            catch (Exception ex)
            { throw new BadRequestException($"Login failed: {ex.Message}"); }
        }

        public async Task<bool> UpdateAccountAsync(BM_Account updateAccount)
        {
            // Find Account of current email access this method
            Account oldAccount = await _unitOfWork.GetAccountRepository().GetByIdAsync(updateAccount.Id)
                ?? throw new NotFoundException("Current Account Access Is Not Exist!");

            oldAccount.FullName = updateAccount.FullName;
            oldAccount.Address = updateAccount.Address;
            oldAccount.PhoneNumber = updateAccount.PhoneNumber;
            oldAccount.Cccd = updateAccount.Cccd;
            oldAccount.DateOfBirth = updateAccount.DateOfBirth;
            oldAccount.Gender = updateAccount.Gender;
            oldAccount.LastUpdated = DateTime.Now;
            oldAccount.Expertise = updateAccount.Expertise;
            oldAccount.Language = updateAccount.Language;
            oldAccount.CompanyName = updateAccount.CompanyName;

            return await _unitOfWork.GetAccountRepository().SaveChangeAsync();
        }

        public async Task<bool> ChangeRoleAccountAsync(BM_Account clientRequest, Guid targetAid)
        {
            CheckValidEmail(clientRequest.Email);

            // Find Account of current email access this method
            var currentAccess = await _unitOfWork.GetAccountRepository().GetOneAsync(acc =>
            acc.Email.ToLower().Equals(clientRequest.Email.ToLower()))
                ?? throw new NotFoundException("Current Account Access Is Not Exist!");

            // Confirm Owner is doing
            if (!currentAccess.Password.Equals(HashStringSHA256(clientRequest.Password)))
            { throw new BadRequestException("Sai mật khẩu! Xác nhận chủ tài khoản lỗi!"); }

            //Default target
            Account targetChangeRole = currentAccess;

            if (targetAid != Guid.Empty)
            {
                //If specific id target is different
                if (currentAccess.Id != targetAid)
                {
                    targetChangeRole = await _unitOfWork.GetAccountRepository().GetByIdAsync(targetAid)
                        ?? throw new NotFoundException("The selected account to delete is not exist!");
                }

                //If non-admin target admin
                if (!currentAccess.Role.ToLower().Equals("admin") ||
                    !currentAccess.Role.ToLower().Equals("nhân viên quản lý"))
                {
                    if (clientRequest.Role.ToLower().Equals("admin") ||
                        clientRequest.Role.ToLower().Equals("nhân viên quản lý"))
                    {
                        throw new UnauthorizedException("You don't have permission to change role of that Account!");
                    }
                }

                targetChangeRole.Role = clientRequest.Role;
            }
            return await _unitOfWork.GetAccountRepository().SaveChangeAsync();
        }

        public async Task<bool> ChangePasswordAccountAsync(BM_Account clientRequest, Guid targetAid, string newPassword)
        {
            CheckValidEmail(clientRequest.Email);

            // Find Account of current email access this method
            var currentAccess = await _unitOfWork.GetAccountRepository().GetOneAsync(acc =>
            acc.Email.ToLower().Equals(clientRequest.Email.ToLower()))
                ?? throw new NotFoundException("Current Account Access Is Not Exist!");

            // Confirm Owner is doing
            if (!currentAccess.Password.Equals(HashStringSHA256(clientRequest.Password)))
            { throw new BadRequestException("Sai mật khẩu! Xác nhận chủ tài khoản lỗi!"); }

            //Default target
            Account targetChangePass = currentAccess;

            if (targetAid != Guid.Empty)
            {
                //If specific id target is different
                if (currentAccess.Id != targetAid)
                {
                    targetChangePass = await _unitOfWork.GetAccountRepository().GetByIdAsync(targetAid)
                        ?? throw new NotFoundException("The selected account to change password is not exist!");
                }

                //If non-admin target admin
                if (!currentAccess.Role.ToLower().Equals("admin") ||
                    !currentAccess.Role.ToLower().Equals("nhân viên quản lý"))
                {
                    if (targetChangePass.Role.ToLower().Equals("admin") ||
                        targetChangePass.Role.ToLower().Equals("nhân viên quản lý"))
                    {
                        throw new UnauthorizedException("You don't have permission to change password of that Account!");
                    }
                }

                targetChangePass.Password = HashStringSHA256(newPassword);
            }
            return await _unitOfWork.GetAccountRepository().SaveChangeAsync();
        }

        public async Task<bool> DeleteAccountAsync(BM_Account clientRequest, Guid targetAid)
        {
            CheckValidEmail(clientRequest.Email);

            // Find Account of current email access this method
            var currentAccess = await _unitOfWork.GetAccountRepository().GetOneAsync(acc =>
            acc.Email.ToLower().Equals(clientRequest.Email.ToLower()))
                ?? throw new NotFoundException("Current Account Access Is Not Exist!");

            // Confirm Owner is doing
            if (!currentAccess.Password.Equals(HashStringSHA256(clientRequest.Password)))
            { throw new BadRequestException("Sai mật khẩu! Xác nhận chủ tài khoản lỗi!"); }

            //Default target
            Account targetAccount = currentAccess;

            if (targetAid != Guid.Empty)
            {
                //If specific id target is different
                if (currentAccess.Id != targetAid)
                {
                    targetAccount = await _unitOfWork.GetAccountRepository().GetByIdAsync(targetAid)
                        ?? throw new NotFoundException("The selected account to delete is not exist!");
                }

                //If non-admin target admin
                if (!currentAccess.Role.ToLower().Equals("admin") ||
                    !currentAccess.Role.ToLower().Equals("nhân viên quản lý"))
                {
                    if (targetAccount.Role.ToLower().Equals("admin") ||
                        targetAccount.Role.ToLower().Equals("nhân viên quản lý"))
                    {
                        throw new UnauthorizedException("You don't have permission to delete of that Account!");
                    }
                }

                // Delete Soft
                targetAccount.Status = "Đã Xóa";
            }
            var result = await _unitOfWork.GetAccountRepository().SaveChangeAsync();

            if (result) await FirebaseAuth.DefaultInstance.DeleteUserAsync(targetAccount.FirebaseUid);

            return result;
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

        private static void CheckValidEmail(string email)
        {
            try
            { var addr = new System.Net.Mail.MailAddress(email); }
            catch
            { throw new BadRequestException("Email không hợp lệ!"); }
        }

        private static void BannedChecker(string status)
        {
            if (status.ToLower().Equals("bị cấm"))
                throw new BadRequestException("Tài khoản Email này đã bị cấm truy cập vào hệ thống!");
        }

        private static bool DeletedChecker(string status)
        {
            return status.ToLower().Equals("đã xóa");
        }

        public Task<string> HandleGoogleAsync(string token, string platform) { throw new NotImplementedException(); }

        public Task<bool> BanAccount(Guid accountId) { throw new NotImplementedException(); }
    }
}
