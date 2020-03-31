using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PersonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task AddPerson(PersonDTO person)
        {
            var personMapped = mapper.Map<Person>(person);
            unitOfWork.PersonRepository.Add(personMapped);
            await unitOfWork.Save();
        }

        public async Task DeletePerson(PersonDTO person)
        {
            var personMapped = mapper.Map<Person>(person);
            unitOfWork.PersonRepository.Delete(personMapped);
            await unitOfWork.Save();
        }

        public async Task<IEnumerable<PersonDTO>> GetAllPersons()
        {
            var persons = await unitOfWork.PersonRepository.GetAll();
            return persons.Select(product => mapper.Map<PersonDTO>(persons));
        }

        public async  Task<PersonDTO> GetPersonById(int id)
        {
            var personFinded = await unitOfWork.PersonRepository.GetById(id);
            return  mapper.Map<PersonDTO>(personFinded);
        }

        public async Task<decimal> TotalBalanceOnAccounts(int id)
        {
            decimal totalBalance = 0;
            var currentPerson = await unitOfWork.PersonRepository.GetById(id);
            if (currentPerson != null)
            {
                totalBalance = currentPerson.Accounts.Select(ac => ac.Balance).Sum();
            }
            return totalBalance;
        }

        public async Task UpdatePerson(PersonDTO person)
        {
            var personMapped = mapper.Map<Person>(person);
            unitOfWork.PersonRepository.Update(personMapped);
            await unitOfWork.Save();
        }
    }
}
