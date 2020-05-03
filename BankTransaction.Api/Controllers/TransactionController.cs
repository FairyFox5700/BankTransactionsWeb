using BankTransaction.Api.Helpers;
using BankTransaction.Api.Models.Queries;
using BankTransaction.BAL.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService transactionService;
        private readonly ILogger<TransactionController> logger;

        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            this.transactionService = transactionService;
            this.logger = logger;
        }
        // GET /api/Transaction
        [HttpGet]
        [Cached(2000)]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAllTransactions([FromQuery]PageQueryParameters pageQueryParameters)
        {
            var transactions = (await transactionService.GetAllTransactions(pageQueryParameters.PageNumber, pageQueryParameters.PageSize)).ToList();
            return Ok(transactions);
        }
        // PUT /api/Transaction/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, TransactionDTO transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }
            var currentTransaction = await transactionService.GetTransactionById(id);
            if (currentTransaction == null)
            {
                return NotFound();
            }

            await transactionService.UpdateTransaction(transaction);
            return Ok(currentTransaction);
        }

        // POST: api/ATransaction
        [HttpPost]
        public async Task<IActionResult> AddTransaction(TransactionDTO transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Object of type transaction is null");
            }
            else
            {
                await transactionService.AddTransaction(transaction);
                return Ok(transaction);
            }
        }
        // DELETE /api/Transaction/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await transactionService.GetTransactionById(id);
            if (transaction == null)
            {
                return NotFound();
            }
            await transactionService.DeleteTransaction(transaction);
            return Ok("Deleted succesfully");
        }
    }
}