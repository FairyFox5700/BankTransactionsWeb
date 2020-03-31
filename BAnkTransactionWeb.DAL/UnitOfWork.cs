using BankTransactionWeb.DAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using BankTransactionWeb.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankTransactionContext context;

        public UnitOfWork(BankTransactionContext context)
        {
            this.context = context;
        }
        private IRepository<Person> personRepository;
        public IRepository<Person> PersonRepository
        {
            get
            {
                if (personRepository == null)
                {
                    personRepository = new BaseRepository<Person>(context);
                }
                return personRepository;
            }
        }

        public IRepository<Account> accountRepository => throw new NotImplementedException();

        public IRepository<Transaction> transactionRepository => throw new NotImplementedException();

        public IRepository<Company> companyRepository => throw new NotImplementedException();

        public IRepository<Shareholder> shareholderRepository => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            context.SaveChangesAsync();
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
