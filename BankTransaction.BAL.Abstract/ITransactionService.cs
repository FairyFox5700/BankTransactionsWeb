﻿using BankTransaction.Models;
using BankTransaction.Models.Validation;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.BAL.Abstract
{
    public interface ITransactionService:IDisposable
    {
        Task<PaginatedModel<TransactionDTO>> GetAllTransactions(int pageNumber, int pageSize);
        Task<TransactionDTO> GetTransactionById(int id);
        Task AddTransaction(TransactionDTO transaction);
        Task<ValidationModel> UpdateTransaction(TransactionDTO transaction);
        Task DeleteTransaction(TransactionDTO transaction);
        //Task<int> TransActionCountByData(DateTime dataOfTrnsaction);

        Task<ValidationModel> ExecuteTransaction(int accountSourceId, string accountDestinationNumber, decimal amount);
        Task<IEnumerable<TransactionDTO>> GetAllUserTransactions(ClaimsPrincipal user );
    }
}
