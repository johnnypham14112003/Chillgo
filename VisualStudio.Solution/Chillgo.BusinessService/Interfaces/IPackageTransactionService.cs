using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.SharedDTOs;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IPackageTransactionService
    {
        Task<BM_FinanceStatistics> FinanceStatistics(DateTime date, int month, int year, bool byDaily);
        Task<Guid> CreateTransaction(CreatePackageTransactionDto transactionDto);
        Task<PackageTransactionDto> GetTransactionById(Guid transactionId);
        Task<List<PackageTransactionDto>> GetAllTransactions();
        Task<List<PackageTransactionDto>> GetTransactionsByUserId(Guid userId); // Lấy danh sách giao dịch theo người dùng
        Task<List<PackageTransactionDto>> GetTransactionsByUserAndPackage(Guid userId, Guid packageId); // Lấy giao dịch theo người dùng và gói
    }
}
