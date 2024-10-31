using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<int> SummaryTotalAccount(string whichType, bool byRole)
        {
            try
            {
                //If null => All account
                if (whichType.ToLower().IsNullOrEmpty()) { return await _context.Accounts.AsNoTracking().CountAsync(); }

                // Count by Role
                if (byRole) { return await _context.Accounts.AsNoTracking().CountAsync(a => a.Role.ToLower().Equals(whichType.ToLower())); }

                // Count by status
                return await _context.Accounts.AsNoTracking().CountAsync(a => a.Status.ToLower().Equals(whichType.ToLower())); ;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<(List<Account>? result, int totalCount)> GetAccountsListAsync
            (string? keyword, string? gender, string? role, string? status)
        {
            try
            {
                var query = _context.Accounts
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable();

                // Apply search
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(acc =>
                    (acc.Email.ToLower().Contains(keyword.ToLower())) ||
                    (acc.FullName.ToLower().Contains(keyword.ToLower())) ||
                    (acc.PhoneNumber.ToLower().Contains(keyword.ToLower())) ||
                    (acc.Cccd.ToLower().Contains(keyword.ToLower())));
                }

                if (!string.IsNullOrEmpty(gender))
                {
                    query = query.Where(acc => acc.Gender.ToLower().Equals(gender!.ToLower()));
                }

                if (!string.IsNullOrEmpty(role))
                {
                    query = query.Where(acc => acc.Role.ToLower().Equals(role!.ToLower()));
                }

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(acc => acc.Status.ToLower().Equals(status!.ToLower()));
                }

                // Sort by Name
                query = query.OrderByDescending(acc => acc.FullName);

                int count = query.Count();

                // Apply paging
                var pagedAccounts = await query.ToListAsync();

                return (pagedAccounts, count);
            }
            catch (Exception)
            {
                return (null, 0);
            }
        }
    }
}
