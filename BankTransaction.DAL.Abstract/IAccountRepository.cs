using BankTransaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Abstract
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetTransactionByDestinationNumber(string accountDestinationNumber);
    }
}
