using BankTransactionWeb.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Person> personRepository { get; }
        IRepository<Account> accountRepository { get; }
        IRepository<Transaction> transactionRepository { get; }
        IRepository<Company> companyRepository { get; }
        IRepository<Shareholder> shareholderRepository { get; }
        Task Save();
    }
}
