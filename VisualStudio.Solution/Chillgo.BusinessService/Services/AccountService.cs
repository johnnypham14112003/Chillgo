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
        public async Task<bool> CreateAccountAsync(BM_Account AccountFromService)
        {
            try
            {
                var existAcc = await _unitOfWork.GetAccountRepository().GetOneAsync(acc => acc.Email.ToLower().Equals(AccountFromService.Email.ToLower()), false);
                if (existAcc is not null)
                {
                    throw new ConflictException("Tài khoản Email này đã tồn tại trong hệ thống");
                }
                string securedPassword = HashStringSHA256(AccountFromService.Password);
                AccountFromService.Password = securedPassword;

                //Create instance of DB model
                Account AccountInDb = new Account();

                //Map data of 'AccountFromService' to 'AccountInDb'
                AccountFromService.Adapt(AccountInDb);

                //Create and get FirebaseUid
                AccountInDb.FirebaseUid = await FirebaseRegisterAccount(AccountFromService); ;

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


        public async Task<int> CountAccount()
        {
            return await _unitOfWork.GetAccountRepository().CountAsync(acc => true);
        }

        //=============================================================================================
        private string HashStringSHA256(string input)//SHA-256 Algorithm (1 way)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
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
    }
}
