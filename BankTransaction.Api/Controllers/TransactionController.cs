using BankTransaction.Api.Helpers;
using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Queries;
using BankTransaction.BAL.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Api.Models.Responces;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Api.Controllers
{
    public class TransactionController : BaseApiController
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
        public async Task<ApiDataResponse<IEnumerable<TransactionDTO>>> GetAllTransactions([FromQuery]PageQueryParameters pageQueryParameters)
        {
            var transactions = (await transactionService.GetAllTransactions(pageQueryParameters.PageNumber, pageQueryParameters.PageSize)).ToList();
            return new ApiDataResponse<IEnumerable<TransactionDTO>>(transactions);
        }
        // PUT /api/Transaction/{id}
        [HttpPut("{id}")]
        public async Task<ApiDataResponse<int>> UpdateTransaction(int id, TransactionDTO transaction)
        {
            if (id != transaction.Id)
            {
                return ApiDataResponse<int>.BadRequest;
            }
            var currentTransaction = await transactionService.GetTransactionById(id);
            if (currentTransaction == null)
            {
                return ApiDataResponse<int>.NotFound;
            }

            await transactionService.UpdateTransaction(transaction);
            return new ApiDataResponse<int>(id);
        }

        // POST: api/ATransaction
        [HttpPost]
        public async Task<ApiDataResponse<TransactionDTO>> AddTransaction(TransactionDTO transaction)
        {
            await transactionService.AddTransaction(transaction);
            return new ApiDataResponse<TransactionDTO>(transaction);
        }
        // DELETE /api/Transaction/{id}
        [HttpDelete("{id}")]
        public async Task<ApiDataResponse<TransactionDTO>> DeleteTransaction(int id)
        {
            var transaction = await transactionService.GetTransactionById(id);
            if (transaction == null)
            {
                return ApiDataResponse<TransactionDTO>.NotFound;
            }
            await transactionService.DeleteTransaction(transaction);
            return new ApiDataResponse<TransactionDTO>(transaction);
        }
    }
}