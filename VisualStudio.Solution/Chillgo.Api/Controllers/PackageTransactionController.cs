using Chillgo.BusinessService.Interfaces;
using Chillgo.BusinessService.SharedDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chillgo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageTransactionController : ControllerBase
    {
        //=================================[ Declares ]================================
        private readonly IServiceFactory _serviceFactory;

        public PackageTransactionController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [Authorize(Roles = "Admin, Nhân Viên Quản Lý")]
        [HttpGet("daily-statistic")]
        public async Task<IActionResult> StatisticTransaction([FromQuery] DateTime date)
        {
            return Ok(await _serviceFactory.GetPackageTransactionService().FinanceStatistics(date, 0, 0, true));
        }

        [Authorize(Roles = "Admin, Nhân Viên Quản Lý")]
        [HttpGet("monthly-statistic")]
        public async Task<IActionResult> StatisticTransaction([FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12 || year <= 0) return BadRequest("Tháng hoặc Năm không hợp lệ!");
            return Ok(await _serviceFactory.GetPackageTransactionService().FinanceStatistics(DateTime.MinValue, month, year, false));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreatePackageTransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transactionId = await _serviceFactory.GetPackageTransactionService().CreateTransaction(transactionDto);

            return Ok(new { TransactionId = transactionId });
        }

        // GET: api/PackageTransaction/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var transaction = await _serviceFactory.GetPackageTransactionService().GetTransactionById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // GET: api/PackageTransaction
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _serviceFactory.GetPackageTransactionService().GetAllTransactions();
            return Ok(transactions);
        }


        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetTransactionsByUserId(Guid userId)
        {
            var transactions = await _serviceFactory.GetPackageTransactionService().GetTransactionsByUserId(userId);
            return Ok(transactions);
        }

        [HttpGet("by-user-package/{userId}/{packageId}")]
        public async Task<IActionResult> GetTransactionsByUserAndPackage(Guid userId, Guid packageId)
        {
            var transactions = await _serviceFactory.GetPackageTransactionService().GetTransactionsByUserAndPackage(userId, packageId);
            return Ok(transactions);
        }
    }
}
