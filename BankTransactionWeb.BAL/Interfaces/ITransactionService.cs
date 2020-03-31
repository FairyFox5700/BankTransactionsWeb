using BankTransactionWeb.BAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDTO>> GetAllCompanies();
        Task<TransactionDTO> GetTransactionById(int id);
        Task AddTransaction(TransactionDTO transaction);
        Task UpdateTransaction(TransactionDTO transaction);
        Task DeleteTransaction(TransactionDTO transaction);
        Task<int> TransActionCountByData(DateTime dataOfTrnsaction);
    }
}
