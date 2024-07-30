using Chillgo.Repository.Interfaces;

namespace Chillgo.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChillgoDbContext _context;
        //==========================================================

        private readonly Lazy<IAccountRepository> _accountRepository;

        public UnitOfWork(ChillgoDbContext context)
        {
            _context = context;
            //======================================================================================

            _accountRepository = new Lazy<IAccountRepository>(() => new AccountRepository(context));
        }

        public IAccountRepository GetAccountRepository()
        {
            return _accountRepository.Value;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
