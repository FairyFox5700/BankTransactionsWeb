using BankTransaction.Entities;
using BankTransaction.DAL.Implementation.InMemoryDAL;
using BankTransaction.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.DAL.Implementation.InMemoryCore;
using BankTransaction.Entities.Filter;

namespace BankTransaction.DAL.Implementation.InMemoryDAL.Repositories.InMemoryRepositories
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

      

        public async Task<PaginatedPlainModel<Account>> GetAll(int startIndex, int pageSize)
        {
            var accounts = await PaginatedPlainModel<Account>.Paginate(container.Accounts.AsQueryable(), startIndex, pageSize);
            return await Task.FromResult(accounts).ConfigureAwait(false);
        }

        public async Task<Account> GetTransactionByDestinationNumber(string accountDestinationNumber)
        {
            var accounts = container.Accounts.Where(e => e.Number == accountDestinationNumber).FirstOrDefault();
            return await Task.FromResult(accounts).ConfigureAwait(false);
        }
    }
}
