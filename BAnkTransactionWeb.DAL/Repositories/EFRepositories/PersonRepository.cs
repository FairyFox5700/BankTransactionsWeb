using BankTransaction.DAL.Abstract;
using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using BankTransaction.DAL.Implementation.Extensions;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Implementation.Repositories.EFRepositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        private readonly BankTransactionContext context;

        public PersonRepository(BankTransactionContext context) : base(context)
        {
            this.context = context;
        }
        public override async Task<IEnumerable<Person>> GetAll()
        {
            return await context.Persons.Include(p => p.ApplicationUser).ToListAsync();
        }
       


        public async Task<IEnumerable<Person>> GetAll(int startIndex, int pageSize)
        {
            return await context.Persons.Include(p => p.ApplicationUser).Paginate<Person>(startIndex, pageSize).ToListAsync();
        }

        public override async Task<Person> GetById(int id)
        {
            try
            {
                var entity = await context.Persons.Include(p => p.ApplicationUser).Include(p => p.Accounts).FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
   
}
