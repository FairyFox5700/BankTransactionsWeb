
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
    public class TransactionInMemoryRepository :ITransactionRepository
    {
        private readonly InMemoryContainer container;

        public TransactionInMemoryRepository(InMemoryContainer container)
        {
            this.container = container;
        }

        public void Add(Transaction entity)
        {
            container.Transactions.Add(entity);
        }
        public void Delete(Transaction entity)
        {
            container.Transactions.Remove(entity);
        }


        public async  Task<PaginatedPlainModel<Transaction>> GetAll(int startIndex, int pageSize)
        {
            var transactions = await PaginatedPlainModel<Transaction>.Paginate(container.Transactions.AsQueryable(), startIndex, pageSize);
            return await Task.FromResult(transactions).ConfigureAwait(false);
        }

     

        public async Task<Transaction> GetById(int id)
        {
            var transaction = container.Transactions.Where(e => e.Id == id).FirstOrDefault();
            return await Task.FromResult<Transaction>(transaction)
                .ConfigureAwait(false);
        }

        public void Update(Transaction entity)
        {
            var entityToUpdate = container.Transactions.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                entityToUpdate.AccountDestinationId = entity.AccountDestinationId;
                entityToUpdate.Amount = entity.Amount;
                entityToUpdate.DateOftransfering = entity.DateOftransfering;
                entityToUpdate.AccountSourceId = entity.AccountSourceId;
                entityToUpdate.SourceAccount = entity.SourceAccount;
            }
        }

        //
       public async Task<IEnumerable<Transaction>> GetAllTransactionsByAccountId(int id)
        {
           var transactions= container.Transactions.Where(tr => (tr.AccountSourceId == id || tr.AccountDestinationId == id));
            return await Task.FromResult<IEnumerable<Transaction>>(transactions)
               .ConfigureAwait(false);
        }
    }
}
