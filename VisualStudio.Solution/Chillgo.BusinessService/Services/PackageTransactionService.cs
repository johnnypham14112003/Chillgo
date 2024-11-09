using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Extensions.Exceptions;
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

        public async Task<BM_FinanceStatistics> FinanceStatistics(DateTime date, int month, int year, bool byDaily)
        {
            if (byDaily)
            {
                try
                {
                    // Lấy tất cả giao dịch trong ngày
                    var transactions = await _unitOfWork.GetPackageTransactionRepository().GetListAsync(t =>
                        t.PaidAt.Date == date.Date);

                    if (transactions.Count == 0)
                        throw new NotFoundException($"Không tìm thấy giao dịch nào trong ngày {date.ToString("dd/MM/yyyy")}");

                    // Tính toán thống kê
                    return new BM_FinanceStatistics
                    {
                        TotalTransactions = transactions.Count,
                        TotalAmount = transactions.Sum(t => t.TotalPrice),
                        PaymentMethodStats = transactions
                            .GroupBy(t => t.PayMethod)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Count()
                            )
                    };
                }
                catch (Exception ex)
                {
                    throw new BadRequestException($"Lỗi khi lấy thống kê theo ngày: {ex.Message}");
                }
            }//End if daily

            try
            {
                // Get first and last day of month
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                // Get transaction in month
                var transactions = await _unitOfWork.GetPackageTransactionRepository().GetListAsync(t =>
                    t.PaidAt >= startDate &&
                    t.PaidAt <= endDate);

                if (transactions.Count == 0)
                    throw new NotFoundException($"Không tìm thấy giao dịch nào trong tháng {month}/{year}");

                // Tính toán thống kê
                return new BM_FinanceStatistics
                {
                    TotalTransactions = transactions.Count,
                    TotalAmount = transactions.Sum(t => t.TotalPrice),
                    PaymentMethodStats = transactions
                        .GroupBy(t => t.PayMethod)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Count()
                        )
                };
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Lỗi khi lấy thống kê theo tháng: {ex.Message}");
            }
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
