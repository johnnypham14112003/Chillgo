using Chillgo.BusinessService.Interfaces;
using Chillgo.BusinessService.SharedDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Chillgo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageTransactionController : ControllerBase
    {
        private readonly IPackageTransactionService _transactionService;

        public PackageTransactionController(IPackageTransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreatePackageTransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transactionId = await _transactionService.CreateTransaction(transactionDto);

            return Ok(new { TransactionId = transactionId });
        }

        // GET: api/PackageTransaction/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var transaction = await _transactionService.GetTransactionById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // GET: api/PackageTransaction
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactions();
            return Ok(transactions);
        }


        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetTransactionsByUserId(Guid userId)
        {
            var transactions = await _transactionService.GetTransactionsByUserId(userId);
            return Ok(transactions);
        }

        [HttpGet("by-user-package/{userId}/{packageId}")]
        public async Task<IActionResult> GetTransactionsByUserAndPackage(Guid userId, Guid packageId)
        {
            var transactions = await _transactionService.GetTransactionsByUserAndPackage(userId, packageId);
            return Ok(transactions);
        }
    }
}
