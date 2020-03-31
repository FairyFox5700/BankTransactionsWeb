using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.InMemoryDAL;
using BankTransactionWeb.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.InMemoryDAL.Repositories
{
    public class AccountInMemoryRepository : IAccountRepository
    {
        private readonly InMemoryContainer container;

        public AccountInMemoryRepository(InMemoryContainer container)
        {
            this.container = container;
        }

        public void Add(Account entity)
        {
            container.Accounts.Add(entity);
        }

        public void Create(Account entity)
        {
            container.Accounts.Add(entity);
        }

        public void Delete(Account entity)
        {
            container.Accounts.Remove(entity);
        }


        public async Task<Account> GetById(int id)
        {
            var account = container.Accounts.Where(e => e.Id == id).FirstOrDefault();
            return await Task.FromResult<Account>(account)
                .ConfigureAwait(false);
        }

        public void Update(Account entity)
        {
            var entityToUpdate = container.Accounts.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                entityToUpdate.Number = entity.Number;
                entityToUpdate.Transactions = entity.Transactions;
                entityToUpdate.Balance = entity.Balance;
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
           var accounts = container.Accounts;
            return await Task.FromResult<ICollection<Account>>(accounts)
                .ConfigureAwait(false);
        }
    }
}
