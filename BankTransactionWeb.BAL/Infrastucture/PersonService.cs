using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void AddPerson(PersonDTO person)
        {
            throw new NotImplementedException();
        }

        public void DeletePerson(PersonDTO person)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PersonDTO>> GetAllPersons()
        {
            throw new NotImplementedException();
        }

        public Task<PersonDTO> GetPersonById(int id)
        {
            throw new NotImplementedException();
        }

        public decimal TotalBalanceOnAccounts(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdatePerson(PersonDTO person)
        {
            throw new NotImplementedException();
        }
    }
}
