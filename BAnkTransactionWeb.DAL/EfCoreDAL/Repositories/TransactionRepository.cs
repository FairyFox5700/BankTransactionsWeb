using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.DAL.EfCoreDAL.Repositories
{
    public class TransactionRepository:BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BankTransactionContext context) : base(context)
        {

        }
    }
}
