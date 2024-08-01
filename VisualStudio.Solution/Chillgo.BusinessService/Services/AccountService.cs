using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Extensions.Exceptions;
using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using Mapster;
using System.Security.Cryptography;
using System.Text;

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
        public async Task<bool> CreateAccountAsync(BM_Account newAccount)
        {
            try
            {
                var existAcc = await _unitOfWork.GetAccountRepository().GetOneAsync(acc => acc.Email.ToLower().Equals(newAccount.Email.ToLower()), false);
                if (existAcc is not null)
                {
                    throw new ConflictException("Tài khoản Email này đã tồn tại trong hệ thống");
                }
                string securedPassword = HashStringSHA256(newAccount.Password);
                newAccount.Password = securedPassword;

                Account RepoAcc = new Account();
                await _unitOfWork.GetAccountRepository().AddAsync(newAccount.Adapt(RepoAcc));
                return await _unitOfWork.GetAccountRepository().SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Đã có lỗi ở hàm CreateAccountAsync: " + ex);
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
    }
}
