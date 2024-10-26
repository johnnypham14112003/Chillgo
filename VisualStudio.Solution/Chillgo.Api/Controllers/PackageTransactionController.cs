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
        private readonly IServiceFactory _serviceFactory;

        public PackageTransactionController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
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
    }
}
