using Chillgo.BusinessService.SharedDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IPackageTransactionService
    {
        Task<Guid> CreateTransaction(CreatePackageTransactionDto transactionDto);
        Task<PackageTransactionDto> GetTransactionById(Guid transactionId);
        Task<List<PackageTransactionDto>> GetAllTransactions();
        Task<List<PackageTransactionDto>> GetTransactionsByUserId(Guid userId); // Lấy danh sách giao dịch theo người dùng
        Task<List<PackageTransactionDto>> GetTransactionsByUserAndPackage(Guid userId, Guid packageId); // Lấy giao dịch theo người dùng và gói
    }
}
