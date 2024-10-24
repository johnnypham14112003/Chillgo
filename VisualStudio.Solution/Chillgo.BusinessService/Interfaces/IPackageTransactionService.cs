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
    }
}
