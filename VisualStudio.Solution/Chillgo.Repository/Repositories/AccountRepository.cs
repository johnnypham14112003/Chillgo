using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;

namespace Chillgo.Repository.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly ChillgoDbContext _context;
        public AccountRepository(ChillgoDbContext context) : base(context)
        {
            _context = context;
        }
        //=====================================================================
        //public async Task<Account?> GetAccountDetail(Guid id)
        //{
        //    return await _context.Accounts.Include(x => x.Customer)
        //                                  .Include(x => x.StakeHolder)
        //                                  .SingleOrDefaultAsync(x => x.Id == id);
        //}
    }
}
