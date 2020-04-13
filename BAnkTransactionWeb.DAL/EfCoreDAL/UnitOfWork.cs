using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.EfCoreDAL.Repositories;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.EfCoreDAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankTransactionContext context;
        private IDbContextTransaction transaction;

        public UnitOfWork(BankTransactionContext context,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> RoleManager)
        {
            this.context = context;
            UserManager = userManager;
            SignInManager = signInManager;
            this.RoleManager = RoleManager;
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

        ITransactionRepository transactionRepository ;
        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (transactionRepository == null)
                {
                    transactionRepository = new  TransactionRepository(context);
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

        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }
        public RoleManager<IdentityRole>  RoleManager { get; }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
        //public void Commit()
        //{
        //    transaction.Commit();
        //}

        //public void Rollback()
        //{
        //    transaction.Rollback();
        //    transaction.Dispose();
        //}

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            transaction = await context.Database.BeginTransactionAsync();
            return transaction;
        }
        //public async Task<IDbContextTransaction> BeginTransaction() 
        //{
        //   return  await context.Database.BeginTransactionAsync();
        //}

        public void  RollbackTransaction()
        {
            transaction.Rollback();
            //context.Database.RollbackTransaction();
        }
        public void CommitTransaction() 
        {
            transaction.Commit();
            //context.Database.CommitTransaction();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
