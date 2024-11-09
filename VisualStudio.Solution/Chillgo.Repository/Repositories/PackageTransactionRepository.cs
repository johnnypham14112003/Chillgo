using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Chillgo.Repository.Repositories
{
    public class PackageTransactionRepository : GenericRepository<PackageTransaction>, IPackageTransactionRepository
    {
        private readonly ChillgoDbContext _context;

        public PackageTransactionRepository(ChillgoDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddTransactionAsync(PackageTransaction transaction)
        {
            await _context.PackageTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<PackageTransaction> GetTransactionByIdAsync(Guid transactionId)
        {
            return await _context.PackageTransactions.FirstOrDefaultAsync(t => t.Id == transactionId);
        }

        public async Task<List<PackageTransaction>> GetAllTransactionsAsync()
        {
            return await _context.PackageTransactions.ToListAsync();
        }

        public async Task<List<PackageTransaction>> GetTransactionsByUserIdAsync(Guid userId)
        {
            return await _context.PackageTransactions
                .Where(t => t.AccountId == userId)
                .ToListAsync();
        }

        public async Task<List<PackageTransaction>> GetTransactionsByUserAndPackageAsync(Guid userId, Guid packageId)
        {
            return await _context.PackageTransactions
                .Where(t => t.AccountId == userId && t.PackageId == packageId)
                .ToListAsync();
        }
    }
}
