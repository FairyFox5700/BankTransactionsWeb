using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using BankTransaction.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Implementation.Repositories.EFRepositories
{
    public class CompanyRepository :BaseRepository<Company>, ICompanyRepository
    {
        private readonly BankTransactionContext context;

        public CompanyRepository(BankTransactionContext context) : base(context)
        {
            this.context = context;
        }
        public async  Task<IEnumerable<Company>> GetAllCompanies()
        {
            return await context.Companies.ToListAsync();
        }
    }
}
