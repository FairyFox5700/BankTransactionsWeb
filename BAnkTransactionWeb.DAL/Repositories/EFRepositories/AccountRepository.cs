using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using BankTransaction.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Implementation.Repositories.EFRepositories
{
    public class AccountRepository :BaseRepository<Account>,IAccountRepository
    {
        private readonly BankTransactionContext context;

        public AccountRepository(BankTransactionContext context) : base(context)
        {
            this.context = context;
           
        }

        public override async Task<IEnumerable<Account>> GetAll()
        {
            try
            {
                var entity = await context.Accounts.Include(p => p.Transactions).ToListAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
