
using BankTransaction.Entities;
using BankTransaction.DAL.Implementation.InMemoryDAL;
using BankTransaction.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.DAL.Implementation.InMemoryCore;

namespace BankTransaction.DAL.Implementation.InMemoryDAL.Repositories.InMemoryRepositories
{
    public class PersonInMemoryRepository : IPersonRepository
    {
        private readonly InMemoryContainer container;

        public PersonInMemoryRepository(InMemoryContainer container)
        {
            this.container = container;
        }

        public void Add(Person entity)
        {
            container.Persons.Add(entity);
        }

        public void Delete(Person entity)
        {
            container.Persons.Remove(entity);
        }

        public async  Task<IEnumerable<Person>> GetAll()
        {
            var persons = container.Persons;
            return await Task.FromResult<ICollection<Person>>(persons)
                .ConfigureAwait(false);
        }

        public async Task<Person> GetById(int id)
        {
            var person = container.Persons.Where(e => e.Id == id).FirstOrDefault();
            return await Task.FromResult<Person>(person)
                .ConfigureAwait(false);
        }

        public void Update(Person entity)
        {
            var entityToUpdate = container.Persons.FirstOrDefault(e => e.Id == entity.Id);
            if(entityToUpdate!=null)
            {
                entityToUpdate.LastName = entity.LastName;
                entityToUpdate.Name = entity.Name;
                entityToUpdate.Surname = entity.Surname;
                entityToUpdate.DataOfBirth = entity.DataOfBirth;
                entityToUpdate.Accounts = entity.Accounts;
            }
        }
    }
}
