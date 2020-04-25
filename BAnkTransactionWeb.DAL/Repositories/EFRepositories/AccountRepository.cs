using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using BankTransaction.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankTransaction.Entities.Filter;

namespace BankTransaction.DAL.Implementation.Repositories.EFRepositories
{
    public class AccountRepository :BaseRepository<Account>,IAccountRepository
    {
        private readonly BankTransactionContext context;

        public AccountRepository(BankTransactionContext context) : base(context)
        {
            this.context = context;
           
        }

        public override async Task<PaginatedPlainModel<Account>> GetAll(int startIndex, int pageSize)
        {
            try
            {
                var accounts = await PaginatedPlainModel<Account>.Paginate(context.Accounts.Include(p => p.Transactions), startIndex, pageSize);
                return accounts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<Account> GetTransactionByDestinationNumber(string accountDestinationNumber)
        {
            return context.Accounts.Include(p => p.Transactions).FirstAsync(e=>e.Number == accountDestinationNumber);
        }
    }
}
