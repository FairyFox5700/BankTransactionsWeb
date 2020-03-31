using BankTransactionWeb.DAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.DAL.Repositories
{
    public class CompanyRepository :BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(BankTransactionContext context) : base(context)
        {

        }
    }
}
