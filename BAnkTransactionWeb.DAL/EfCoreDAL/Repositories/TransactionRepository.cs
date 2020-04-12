using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;

namespace BankTransactionWeb.DAL.EfCoreDAL.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        private readonly BankTransactionContext context;

        public TransactionRepository(BankTransactionContext context) : base(context)
        {
            this.context = context;
        }



    }
}
