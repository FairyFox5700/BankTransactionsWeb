using BankTransactionWeb.BAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
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
        Task<IEnumerable<TransactionDTO>> GetAllUserTransactions(int userId);
    }
}
