using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.EfCoreDAL.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        private readonly BankTransactionContext context;

        public TransactionRepository(BankTransactionContext context) : base(context)
        {
            this.context = context;
        }
        public override async Task<IEnumerable<Transaction>> GetAll()
        {
            return await context.Transactions.Include(p => p.SourceAccount).ToListAsync();
        }



    }
}
