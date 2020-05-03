using System;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.DAL.Abstract;
using BankTransaction.DAL.Implementation.Core.InMemoryCore;
using BankTransaction.DAL.Implementation.Extensions;
using BankTransaction.Entities;
using BankTransaction.Entities.Filter;

namespace BankTransaction.DAL.Implementation.Repositories.InMemoryRepositories
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

       

      
        public async Task<PaginatedPlainModel<Shareholder>> GetAll(int startIndex, int pageSize, ShareholderFilter shareholderFilter = null)
        {
            var filteredShareholders =SearchByFilters(shareholderFilter, container.Shareholders.AsQueryable());
            var shareholders = await PaginatedPlainModel<Shareholder>.Paginate(filteredShareholders, startIndex, pageSize);
            return await Task.FromResult(shareholders).ConfigureAwait(false);

        }
        private IQueryable<Shareholder> SearchByFilters(ShareholderFilter shareholderFilter, IQueryable<Shareholder> shareholders)
        {
            if (shareholderFilter != null)
            {
                if (!String.IsNullOrEmpty(shareholderFilter?.CompanyName))
                {
                    shareholders = shareholders.Where(s => s.Company.Name.Contains(shareholderFilter.CompanyName));
                }
                if (shareholderFilter?.DateOfCompanyCreation != null)
                {
                    shareholders = shareholders.Where(s => s.Company.DateOfCreation.EqualsUpToSeconds(shareholderFilter.DateOfCompanyCreation));
                }
            }
            return  shareholders;
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

        public async Task<PaginatedPlainModel<Shareholder>> GetAll(int startIndex, int pageSize)
        {
            var shareholders = await PaginatedPlainModel<Shareholder>.Paginate(container.Shareholders.AsQueryable(), startIndex, pageSize);
            return await Task.FromResult(shareholders).ConfigureAwait(false);
        }
    }
}
