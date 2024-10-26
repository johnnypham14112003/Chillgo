using Chillgo.BusinessService.Interfaces;
using Chillgo.BusinessService.SharedDTOs;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;

namespace Chillgo.BusinessService.Services
{
    public class PackageTransactionService : IPackageTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateTransaction(CreatePackageTransactionDto transactionDto)
        {
            var transaction = new PackageTransaction
            {
                Id = Guid.NewGuid(),
                AccountId = transactionDto.AccountId,
                PackageId = transactionDto.PackageId,
                ChillCoinApplied = transactionDto.ChillCoinApplied,
                PaidAt = DateTime.Now,
                PayMethod = transactionDto.PayMethod,
                TotalPrice = transactionDto.TotalPrice,
                StartDate = transactionDto.StartDate,
                EndDate = transactionDto.EndDate,
                VoucherCodeList = transactionDto.VoucherCodeList,
                Status = transactionDto.Status
            };

            await _unitOfWork.GetPackageTransactionRepository().AddTransactionAsync(transaction);
            return transaction.Id;
        }

        //

        public async Task<PackageTransactionDto> GetTransactionById(Guid transactionId)
        {
            var transaction = await _unitOfWork.GetPackageTransactionRepository().GetTransactionByIdAsync(transactionId);
            if (transaction == null) return null;

            return new PackageTransactionDto
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId,
                PackageId = transaction.PackageId,
                ChillCoinApplied = transaction.ChillCoinApplied,
                PayMethod = transaction.PayMethod,
                TotalPrice = transaction.TotalPrice,
                StartDate = transaction.StartDate,
                EndDate = transaction.EndDate,
                Status = transaction.Status
            };
        }

        public async Task<List<PackageTransactionDto>> GetAllTransactions()
        {
            var transactions = await _unitOfWork.GetPackageTransactionRepository().GetAllTransactionsAsync();
            return transactions.Select(transaction => new PackageTransactionDto
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId,
                PackageId = transaction.PackageId,
                ChillCoinApplied = transaction.ChillCoinApplied,
                PayMethod = transaction.PayMethod,
                TotalPrice = transaction.TotalPrice,
                StartDate = transaction.StartDate,
                EndDate = transaction.EndDate,
                Status = transaction.Status
            }).ToList();
        }
    }
}
