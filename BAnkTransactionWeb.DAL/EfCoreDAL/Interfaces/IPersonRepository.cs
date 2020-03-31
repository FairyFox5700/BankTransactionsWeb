using BankTransactionWeb.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.DAL.Interfaces
{
    public interface IPersonRepository:IRepository<Person>
    {
        //Task<IEnumerable<Person>> GetAllPersons();
        //Task<Person> GetPersonById(int id);
        //void AddPerson(Person person);
        //void UpdatePerson(Person person);
        //void DeletePerson(Person person);
    }
}
