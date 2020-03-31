using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.DAL.EfCoreDAL.Repositories
{
    public class CompanyRepository :BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(BankTransactionContext context) : base(context)
        {

        }
    }
}
