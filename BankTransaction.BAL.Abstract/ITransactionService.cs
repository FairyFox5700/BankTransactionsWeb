using BankTransaction.BAL.Implementation.DTOModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Abstract
{
    public interface ITransactionService:IDisposable
    {
        Task<IEnumerable<TransactionDTO>> GetAllTransactions();
        Task<TransactionDTO> GetTransactionById(int id);
        Task AddTransaction(TransactionDTO transaction);
        Task UpdateTransaction(TransactionDTO transaction);
        Task DeleteTransaction(TransactionDTO transaction);
        Task<int> TransActionCountByData(DateTime dataOfTrnsaction);
        Task ExecuteTransaction(int accountSourceId, string accountDestinationNumber, decimal amount);
        Task<IEnumerable<TransactionDTO>> GetAllUserTransactions(ClaimsPrincipal user );
    }
}
