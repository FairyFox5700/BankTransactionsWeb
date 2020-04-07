using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.EfCoreDAL.Repositories;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Identity;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.EfCoreDAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankTransactionContext context;

        public UnitOfWork(BankTransactionContext context, ApplicationUserManager applicationUserManager, ApplicationRoleManager applicationRoleManager)//, ApplicationUserManager applicationUserManager, ApplicationRoleManager applicationRoleManager
        {
            this.context = context;
            this.AppUserManager = applicationUserManager;
            this.AppRoleManager = applicationRoleManager;
            //= new ApplicationUserManager(new UserStore<ApplicationUser>(context), new Options<IdentityOptions()>, new PasswordHasher<ApplicationUser>(), new IEnumerable<UserValidator<ApplicationUser>>(), new IEnumerable<PasswordValidator<ApplicationUser>>(), new ILookupNormalizer(), new IdentityErrorDescriber(), new IServiceProvider, new Logger<UserManager<ApplicationUser>>()); )

        }

        IPersonRepository personRepository;
        public IPersonRepository PersonRepository
        {
            get
            {
                if (personRepository == null)
                {
                    personRepository = new PersonRepository(context);
                }
                return personRepository;
            }
        }


        IAccountRepository accountRepository;
        public IAccountRepository AccountRepository
        {
            get
            {
                if (accountRepository == null)
                {
                    accountRepository = new AccountRepository(context);
                }
                return accountRepository;
            }
        }

        ITransactionRepository transactionRepository;
        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (transactionRepository == null)
                {
                    transactionRepository = new TransactionRepository(context);
                }
                return transactionRepository;
            }
        }
        ICompanyRepository companyRepository;
        public ICompanyRepository CompanyRepository
        {
            get
            {
                if (companyRepository == null)
                {
                    companyRepository = new CompanyRepository(context);
                }
                return companyRepository;
            }
        }

        IShareholderRepository shareholderRepository;
        public IShareholderRepository ShareholderRepository
        {
            get
            {
                if (shareholderRepository == null)
                {
                    shareholderRepository = new ShareholderRepository(context);
                }
                return shareholderRepository;
            }
        }



        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await context.Database.BeginTransactionAsync();
        }

        public void RollbackTransaction()
        {
            context.Database.RollbackTransaction();
        }
        public void CommitTransaction()
        {
            context.Database.CommitTransaction();
        }

        public ApplicationUserManager AppUserManager { get; }

        public ApplicationRoleManager AppRoleManager { get; }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();

                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            AppUserManager.Dispose();
            AppRoleManager.Dispose();
            context.Dispose();
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
