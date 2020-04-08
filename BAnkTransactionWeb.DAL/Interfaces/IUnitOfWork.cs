using BankTransactionWeb.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        UserManager<ApplicationUser> UserManager { get; }
        SignInManager<ApplicationUser> SignInManager { get; }
        RoleManager<ApplicationRole> RoleManager { get; }
        IPersonRepository PersonRepository { get; }
        IAccountRepository AccountRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IShareholderRepository ShareholderRepository { get; }
        Task Save();
        void CommitTransaction();
        void RollbackTransaction();
        Task<IDbContextTransaction> BeginTransaction();

    }
}
