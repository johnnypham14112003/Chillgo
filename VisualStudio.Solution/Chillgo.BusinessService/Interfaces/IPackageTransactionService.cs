using Chillgo.BusinessService.SharedDTOs;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IPackageTransactionService
    {
        Task<Guid> CreateTransaction(CreatePackageTransactionDto transactionDto);
        Task<PackageTransactionDto> GetTransactionById(Guid transactionId);
        Task<List<PackageTransactionDto>> GetAllTransactions();
    }
}
