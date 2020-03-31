using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
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
    }
}
