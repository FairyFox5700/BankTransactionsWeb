﻿using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var transactions = (await transactionService.GetAllTransactions()).ToList();
            return transactions;
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

            await transactionService.UpdateTransaction(transaction);
            return Ok(currentTransaction);
        }

        // POST: api/ATransaction
        [HttpPost]
        public async Task<IActionResult> AddTransaction(TransactionDTO transaction)
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
        // DELETE /api/Transaction/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await transactionService.GetTransactionById(id);
            if (transaction == null)
            {
                logger.LogError($"Transaction with id {id} not find");
                return NotFound();
            }
            await transactionService.DeleteTransaction(transaction);
            return Ok("Deleted succesfully");
        }
    }
}