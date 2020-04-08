using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.EfCoreDAL.Repositories
{
    public class PersonRepository :BaseRepository<Person>,IPersonRepository
    {
        private readonly BankTransactionContext context;

        public PersonRepository(BankTransactionContext context):base(context)
        {
            this.context = context;
        }

        public override async Task<Person> GetById(int id)
        {
            try
            {
                var entity = await context.Persons.Include(p=>p.ApplicationUser).FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
