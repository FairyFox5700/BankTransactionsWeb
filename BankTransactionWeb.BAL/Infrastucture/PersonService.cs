using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class PersonService : IPersonService, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<PersonService> logger;

        public PersonService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PersonService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task AddPerson(PersonDTO person)
        {
            try
            {
                var personMapped = mapper.Map<Person>(person);
                if(personMapped==null)
                {
                    logger.LogError($"In method {nameof(AddPerson)} instance of person is not mapped properly");
                }
                else
                {
                    unitOfWork.PersonRepository.Add(personMapped);
                    await unitOfWork.Save();
                    logger.LogInformation($"In method {nameof(AddPerson)} instance of person successfully added");
                }
            }
            catch(Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddPerson)}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;
               
            }

        }

        public async Task DeletePerson(PersonDTO person)
        {
            var personMapped = mapper.Map<Person>(person);
            unitOfWork.PersonRepository.Delete(personMapped);
            await unitOfWork.Save();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public async Task<List<PersonDTO>> GetAllPersons()
        {
            var persons = await unitOfWork.PersonRepository.GetAll();
            return persons.Select(p => mapper.Map<PersonDTO>(p)).ToList();
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
