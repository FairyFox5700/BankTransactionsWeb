using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using BankTransaction.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.DAL.Implementation.Repositories.EFRepositories
{
    public class CompanyRepository :BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(BankTransactionContext context) : base(context)
        {

        }
    }
}
