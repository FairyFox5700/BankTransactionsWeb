﻿using System;
using System.Threading.Tasks;
using BankTransaction.DAL.Abstract;
using BankTransaction.DAL.Implementation.Repositories.InMemoryRepositories;
using BankTransaction.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankTransaction.DAL.Implementation.Core.InMemoryCore
{
    public class UnitOfWorkInMemory : IUnitOfWork
    {
        private readonly InMemoryContainer context;

        public UnitOfWorkInMemory(InMemoryContainer context)
        {
            this.context = context;
        }

        IPersonRepository personRepository;
        public IPersonRepository PersonRepository
        {
            get
            {
                if (personRepository == null)
                {
                    personRepository = new PersonInMemoryRepository(context);
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
                    accountRepository = new AccountInMemoryRepository(context);
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
                    transactionRepository = new TransactionInMemoryRepository(context);
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
                    companyRepository = new CompanyInMemoryRepository(context);
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
                    shareholderRepository = new ShareholderInMemoryRepository(context);
                }
                return shareholderRepository;
            }
        }

        public UserManager<ApplicationUser> UserManager => throw new NotImplementedException();

        public SignInManager<ApplicationUser> SignInManager => throw new NotImplementedException();

        public RoleManager<IdentityRole> RoleManager => throw new NotImplementedException();

        public ITokenRepository TokenRepository => throw new NotImplementedException();

        public async Task Save()
        {
             await Task.FromResult(0)
                .ConfigureAwait(false);
        }


        public void Dispose()
        {
            /*
             * As long as your Memory db context no dependencies like a db connection or file resources that should be freed on dispose, 
             * you can leave the dispose empty. All references that are out of scope will be collected automatically 
             * when there is no reference to the object.
             * source https://stackoverflow.com/questions/45740218/necessity-of-dispose-method-mocking-ef-core-with-the-in-memory-provider
             */
        }

        public void CommitTransaction()
        {
            throw new NotImplementedException();
        }

        public void RollbackTransaction()
        {
            throw new NotImplementedException();
        }

        public Task<IDbContextTransaction> BeginTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
