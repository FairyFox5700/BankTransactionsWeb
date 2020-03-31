using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using BankTransactionWeb.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        public void Add(Person entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Person entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Person>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Person entity)
        {
            throw new NotImplementedException();
        }
    }
}
