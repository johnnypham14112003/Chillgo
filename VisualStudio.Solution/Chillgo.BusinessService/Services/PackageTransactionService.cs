using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Interfaces;
using Chillgo.BusinessService.SharedDTOs;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using System;

namespace Chillgo.BusinessService.Services
{
    public class PackageTransactionService : IPackageTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BM_FinanceStatistics> FinanceStatistics(string DayTime)
        {
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);

            // Tổng số lượng các PackageTransaction
            int totalPackagesSold = await _unitOfWork.GetPackageTransactionRepository().CountAsync(pack => true);

            // Tổng số lượng và doanh thu theo từng trường hợp thời gian
            int totalPackagesSoldByDay;
            decimal totalRevenueByDay;

            if (DayTime.ToLower() == "today")
            {
                totalPackagesSoldByDay = await _unitOfWork.GetPackageTransactionRepository().CountAsync(pack => pack.PaidAt >= today);
                totalRevenueByDay = await _unitOfWork.GetPackageTransactionRepository().SumAsync(pack => pack.PaidAt >= today ? pack.TotalPrice : 0);
            }
            else if (DayTime.ToLower() == "yesterday")
            {
                totalPackagesSoldByDay = await _unitOfWork.GetPackageTransactionRepository().CountAsync(pack => pack.PaidAt >= yesterday && pack.PaidAt < today);
                totalRevenueByDay = await _unitOfWork.GetPackageTransactionRepository().SumAsync(pack => (pack.PaidAt >= yesterday && pack.PaidAt < today) ? pack.TotalPrice : 0);
            }
            else if (DayTime.ToLower() == "month")
            {
                totalPackagesSoldByDay = await _unitOfWork.GetPackageTransactionRepository().CountAsync(pack => pack.PaidAt >= startOfMonth);
                totalRevenueByDay = await _unitOfWork.GetPackageTransactionRepository().SumAsync(pack => pack.PaidAt >= startOfMonth ? pack.TotalPrice : 0);
            }
            else
            {
                throw new ArgumentException("Invalid DayTime parameter");
            }

            return new BM_FinanceStatistics
            {
                TotalPackagesSold = totalPackagesSold,
                TotalPackagesSoldByDay = totalPackagesSoldByDay,
                Commission = 0,
                RevenueCash = totalRevenueByDay,
                RevenueByDay = totalRevenueByDay,
                DayTime = DayTime
            };
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

        public async Task<List<PackageTransactionDto>> GetTransactionsByUserId(Guid userId)
        {
            var transactions = await _unitOfWork.GetPackageTransactionRepository().GetTransactionsByUserIdAsync(userId);
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

        public async Task<List<PackageTransactionDto>> GetTransactionsByUserAndPackage(Guid userId, Guid packageId)
        {
            var transactions = await _unitOfWork.GetPackageTransactionRepository().GetTransactionsByUserAndPackageAsync(userId, packageId);
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
