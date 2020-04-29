using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using BankTransaction.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankTransaction.DAL.Implementation.Extensions;
using BankTransaction.Entities.Filter;

namespace BankTransaction.DAL.Implementation.Repositories.EFRepositories
{
    public class ShareholderRepository :BaseRepository<Shareholder>,  IShareholderRepository
    {
        private readonly BankTransactionContext context;

        public ShareholderRepository(BankTransactionContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<PaginatedPlainModel<Shareholder>> GetAll(int startIndex, int pageSize, ShareholderFilter shareholderFilter = null)
        {
            var filteredShareholders = SearchByFilters(shareholderFilter, context.Shareholders.Include(c => c.Company).Include(c=>c.Person).AsQueryable());
            var shareholders = await PaginatedPlainModel<Shareholder>.Paginate(filteredShareholders, startIndex, pageSize);
            return shareholders;

        }
        private IQueryable<Shareholder> SearchByFilters(ShareholderFilter shareholderFilter, IQueryable<Shareholder> shareholders)
        {
            if (shareholderFilter != null)
            {
                if (!String.IsNullOrEmpty(shareholderFilter?.CompanyName))
                {
                    shareholders = shareholders.Where(s => s.Company.Name.Contains(shareholderFilter.CompanyName));
                }
                if (shareholderFilter?.DateOfCompanyCreation != new DateTime())
                {
                    shareholders = shareholders.Where(s => s.Company.DateOfCreation.EqualsUpToSeconds(shareholderFilter.DateOfCompanyCreation));
                }
            }
            return shareholders;
        }
      

        
        public override async Task<PaginatedPlainModel<Shareholder>> GetAll(int startIndex, int pageSize)
        {
            var shareholders = await PaginatedPlainModel<Shareholder>.Paginate(context.Shareholders.Include(c => c.Company).Include(e => e.Person), startIndex, pageSize);
            return shareholders;
        }
        public override async Task<Shareholder> GetById(int id)
        {
            try
            {
                var entity = await context.Shareholders.Include(c => c.Company).Include(e => e.Person).FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
