using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using BankTransaction.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Implementation.Repositories.EFRepositories
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
