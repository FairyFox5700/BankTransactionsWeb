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
    public class CompanyInMemoryRepository : ICompanyRepository
    {
        private readonly InMemoryContainer container;

        public CompanyInMemoryRepository(InMemoryContainer container)
        {
            this.container = container;
        }

        public void Add(Company entity)
        {
            container.Companies.Add(entity);
        }

        public void Delete(Company entity)
        {
            container.Companies.Remove(entity);
        }

        public async Task<Company> GetById(int id)
        {
            var company = container.Companies.Where(e => e.Id == id).FirstOrDefault();
            return await Task.FromResult<Company>(company)
                .ConfigureAwait(false);
        }

        public void Update(Company entity)
        {
            var entityToUpdate = container.Companies.FirstOrDefault(e => e.Id == entity.Id);
            if (entityToUpdate != null)
            {
                entityToUpdate.DateOfCreation = entity.DateOfCreation;
                entityToUpdate.Name = entity.Name;
                entityToUpdate.Shareholders = entity.Shareholders;
            }
        }

        public async Task<IEnumerable<Company>> GetAll()
        {
            var companies = container.Companies;
            return await Task.FromResult<ICollection<Company>>(companies)
                .ConfigureAwait(false);
        }

        public async Task<PaginatedPlainModel<Company>> GetAll(int startIndex, int pageSize)
        {
            var comapnies = await PaginatedPlainModel<Company>.Paginate(container.Companies.AsQueryable(), startIndex, pageSize);
            return await Task.FromResult(comapnies).ConfigureAwait(false);
        }
    }
}
