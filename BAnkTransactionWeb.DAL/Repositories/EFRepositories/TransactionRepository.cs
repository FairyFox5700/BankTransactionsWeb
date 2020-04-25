using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using BankTransaction.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankTransaction.Entities.Filter;
using System.Linq;

namespace BankTransaction.DAL.Implementation.Repositories.EFRepositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        private readonly BankTransactionContext context;

        public TransactionRepository(BankTransactionContext context) : base(context)
        {
            this.context = context;
        }
        public override  async Task<PaginatedPlainModel<Transaction>> GetAll(int startIndex, int pageSize)
        {
            var transactions = await PaginatedPlainModel<Transaction>.Paginate(context.Transactions.Include(p => p.SourceAccount), startIndex, pageSize);
            return transactions;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsByAccountId(int id)
        {
            return await context.Transactions.Where(tr => (tr.AccountSourceId == id || tr.AccountDestinationId == id)).ToListAsync();
           
        }
    }
}
