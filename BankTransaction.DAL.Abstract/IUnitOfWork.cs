
using BankTransaction.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        UserManager<ApplicationUser> UserManager { get; }
        SignInManager<ApplicationUser> SignInManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        IPersonRepository PersonRepository { get; }
        IAccountRepository AccountRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IShareholderRepository ShareholderRepository { get; }
        ITokenRepository TokenRepository { get; }
        Task Save();
        void CommitTransaction();
        void RollbackTransaction();
        Task<IDbContextTransaction> BeginTransaction();

    }
}
