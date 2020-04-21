
using BankTransaction.Entities;
using BankTransaction.DAL.Implementation.InMemoryDAL;
using BankTransaction.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.DAL.Implementation.InMemoryCore;
using BankTransaction.DAL.Implementation.Extensions;
using BankTransaction.Entities.Filter;

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



        public async Task<IEnumerable<Person>> GetAll(int startIndex, int pageSize, PersonFilter personFilter = null)
        {
            var persons = container.Persons.Paginate(startIndex, pageSize);
            var filteredPersons = SearchByFilters(personFilter, persons);
            return await Task.FromResult<IEnumerable<Person>>(filteredPersons).ConfigureAwait(false); ;
        }

        private IEnumerable<Person> SearchByFilters(PersonFilter personFilter, IEnumerable<Person> persons)
        {
            if (personFilter != null)
            {
                if (!String.IsNullOrEmpty(personFilter?.Name))
                {
                    persons = persons.Where(s => s.Name.Contains(personFilter.Name));
                }
                if (!String.IsNullOrEmpty(personFilter?.Surname))
                {
                    persons = persons.Where(s => s.Surname.Contains(personFilter.Surname));
                }
                if (!String.IsNullOrEmpty(personFilter?.LastName))
                {
                    persons = persons.Where(s => s.LastName.Contains((personFilter.LastName)));
                }
                if (!String.IsNullOrEmpty(personFilter?.AccountNumber))
                {
                    persons = persons.Where(s => s.Accounts.Select(e => e.Number).Contains(personFilter.AccountNumber));
                }
                if (!String.IsNullOrEmpty(personFilter?.TransactionNumber))
                {
                    persons = persons.Where(p => p.Accounts.Contains(p.Accounts.Where
                        (a => a.Transactions.Contains(a.Transactions.Where
                        (e => e.Id.ToString() == personFilter.TransactionNumber).FirstOrDefault())).FirstOrDefault()));
                }
                if (!String.IsNullOrEmpty(personFilter?.CompanyName))
                {
                    persons = container.Shareholders.Where(sh => sh.Company.Name.Contains(personFilter.CompanyName)).Select(sh => sh.Person);
                }
            }
            return persons;
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
