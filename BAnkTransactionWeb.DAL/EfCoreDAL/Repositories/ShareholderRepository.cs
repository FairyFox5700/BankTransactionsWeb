using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.EfCoreDAL.Repositories
{
    public class ShareholderRepository :BaseRepository<Shareholder>,  IShareholderRepository
    {
        private readonly BankTransactionContext context;

        public ShareholderRepository(BankTransactionContext context) : base(context)
        {
            this.context = context;
        }

        public override async Task<IEnumerable<Shareholder>> GetAll()
        {
            return await context.Shareholders.Include(c => c.Company).Include(e => e.Person).ToListAsync();
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
