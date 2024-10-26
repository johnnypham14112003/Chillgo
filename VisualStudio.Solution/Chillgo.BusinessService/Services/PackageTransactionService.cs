using Chillgo.BusinessService.Interfaces;
using Chillgo.BusinessService.SharedDTOs;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService.Services
{
    public class PackageTransactionService : IPackageTransactionService
    {
        private readonly IPackageTransactionRepository _transactionRepository;

        public PackageTransactionService(IPackageTransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
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

            await _transactionRepository.AddTransactionAsync(transaction);
            return transaction.Id;
        }

        //

        public async Task<PackageTransactionDto> GetTransactionById(Guid transactionId)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
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
            var transactions = await _transactionRepository.GetAllTransactionsAsync();
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
