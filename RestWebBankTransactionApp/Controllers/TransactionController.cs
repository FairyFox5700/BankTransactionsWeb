using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RestWebBankTransactionApp.Controllers
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
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAllTransactions()
        {
            try
            {
                var transactions = (await transactionService.GetAllTransactions()).ToList();
                logger.LogInformation("Successfully returned all transactions");
                return transactions;
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllTransactions)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
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
                logger.LogError($"Transaction with id {id} not find");
                return NotFound();
            }
            try
            {
                await transactionService.UpdateTransaction(transaction);
                return Ok(currentTransaction);
            }
            catch (Exception ex)
            {
                logger.LogError($"Unable to update transaction becuase of {ex.Message}");
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists, " +
                "see your system administrator.");
                return NotFound();
            }

        }

        // POST: api/ATransaction
        [HttpPost]
        public async Task<IActionResult> AddTransaction(TransactionDTO transaction)
        {
            try
            {
                if (transaction == null)
                {
                    logger.LogError("Object of type transaction send by client was null.");
                    return BadRequest("Object of type transaction is null");
                }
                else
                {
                    await transactionService.AddTransaction(transaction);
                    return Ok(transaction);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddTransaction)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }
        // DELETE /api/Transaction/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                var transaction = await transactionService.GetTransactionById(id);
                if (transaction == null)
                {
                    logger.LogError($"Transaction with id {id} not find");
                    return NotFound();
                }
                try
                {
                    await transactionService.DeleteTransaction(transaction);
                    return Ok("Deleted succesfully");
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Unable to update transaction becuase of {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteTransaction)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}