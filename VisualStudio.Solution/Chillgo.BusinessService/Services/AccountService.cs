using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;

namespace Chillgo.BusinessService.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceFactory _serviceFactory;

        public AccountService(IUnitOfWork unitOfWork, IServiceFactory serviceFactory)
        {
            _unitOfWork = unitOfWork;
            _serviceFactory = serviceFactory;
        }
        //=============================================================================

        public async Task<Account?> GetAccountsById(Guid id)
        {
            var accounts = await _unitOfWork.GetAccountRepository().GetByIdAsync(id);
            return accounts;
        }

    }
}
