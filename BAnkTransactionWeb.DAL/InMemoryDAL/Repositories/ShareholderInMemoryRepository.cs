
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.InMemoryDAL;
using BankTransactionWeb.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.InMemoryDAL.Repositories
{
    public class ShareholderInMemoryRepository : IShareholderRepository
    {
        private readonly InMemoryContainer container;

        public ShareholderInMemoryRepository(InMemoryContainer container)
        {
            this.container = container;
        }

        public void Add(Shareholder entity)
        {
            container.Shareholders.Add(entity);
        }


        public void Delete(Shareholder entity)
        {
            container.Shareholders.Remove(entity);
        }

        public async Task<IEnumerable<Shareholder>> GetAll()
        {
            var shareholders = container.Shareholders;
            return await Task.FromResult<ICollection<Shareholder>>(shareholders)
                .ConfigureAwait(false);
        }

        public async Task<Shareholder> GetById(int id)
        {
            var shareholder = container.Shareholders.Where(e => e.Id == id).FirstOrDefault();
            return await Task.FromResult<Shareholder>(shareholder)
                .ConfigureAwait(false);
        }

        public void Update(Shareholder entity)
        {
            var entityToUpdate = container.Shareholders.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                entityToUpdate.Company = entity.Company;
                entityToUpdate.Person = entity.Person;

            }
        }
    }
}
