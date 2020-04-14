using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class PersonService : IPersonService
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
                unitOfWork.PersonRepository.Add(personMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(AddPerson)} instance of person successfully added");
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddPerson)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }

        }

        //Unit of work transaction
        public async Task<IdentityResult> DeletePerson(PersonDTO person)
        {
            using (var trans = await unitOfWork.BeginTransaction())
            {
                try
                {

                    var user = await unitOfWork.UserManager.FindByIdAsync(person.ApplicationUserFkId);
                    if (user != null)
                    {
                        var personMapped = mapper.Map<Person>(person);
                        //personMapped.ApplicationUserFkId = null;
                        //unitOfWork.PersonRepository.Update(personMapped);
                        //await unitOfWork.Save();
                        unitOfWork.PersonRepository.Delete(personMapped);
                        await unitOfWork.Save();
                        var result = await unitOfWork.UserManager.DeleteAsync(user);
                        unitOfWork.CommitTransaction();
                        logger.LogInformation($"In method {nameof(DeletePerson)} instance of person successfully deleted");
                        return result;
                        
                        //return result;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Catch an exception in method {nameof(DeletePerson)} in class {nameof(PersonService)}. The exception is {ex.Message}. " +
                       $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                    unitOfWork.RollbackTransaction();
                    throw ex;

                }
            }
            }


            public void Dispose()
            {
                unitOfWork.Dispose();
            }

            public async Task<List<PersonDTO>> GetAllPersons(string name = null, string surname = null, string lastname = null,
                string accountNumber = null, string transactionNumber = null, string companyName = null)
            {
                try
                {
                    var persons = await unitOfWork.PersonRepository.GetAll();
                    if (!String.IsNullOrEmpty(name))
                    {
                        persons = persons.Where(s => s.Name.Contains(name));
                    }
                    if (!String.IsNullOrEmpty(surname))
                    {
                        persons = persons.Where(s => s.Surname.Contains(surname));
                    }
                    if (!String.IsNullOrEmpty(lastname))
                    {
                        persons = persons.Where(s => s.LastName.Contains(lastname));
                    }
                    if (!String.IsNullOrEmpty(accountNumber))
                    {
                        persons = persons.Where(s => s.Accounts.Select(e => e.Number).Contains(accountNumber));
                    }
                    if (!String.IsNullOrEmpty(transactionNumber))
                    {
                        persons = persons.Where(p => p.Accounts.Contains(p.Accounts.Where
                            (a => a.Transactions.Contains(a.Transactions.Where
                            (e => e.Id.ToString() == transactionNumber).FirstOrDefault())).FirstOrDefault()));
                    }
                    if (!String.IsNullOrEmpty(companyName))
                    {
                        persons = (await unitOfWork.ShareholderRepository.GetAll()).Where(sh => sh.Company.Name.Contains(companyName)).Select(sh => sh.Person);
                    }
                    return persons.Select(p => mapper.Map<PersonDTO>(p)).ToList();
                }
                catch (Exception ex)
                {
                    logger.LogError($"Catch an exception in method {nameof(GetAllPersons)} in class {this.GetType()}. The exception is {ex.Message}. " +
                       $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                    throw ex;

                }

            }

            public async Task<PersonDTO> GetPersonById(int id)
            {
                try
                {
                    var personFinded = await unitOfWork.PersonRepository.GetById(id);
                    var appUser = personFinded.ApplicationUser;
                    var personModel = mapper.Map<ApplicationUser, PersonDTO>(appUser);
                    return mapper.Map(personFinded, personModel);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Catch an exception in method {nameof(GetPersonById)} in class {this.GetType()}. The exception is {ex.Message}. " +
                       $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                    throw ex;

                }
            }

            public async Task<PersonDTO> GetPersonById(ClaimsPrincipal user)
            {
                try
                {
                    var id = unitOfWork.UserManager.GetUserId(user);
                    var personFinded = (await unitOfWork.PersonRepository.GetAll()).Where(e => e.ApplicationUserFkId == id).FirstOrDefault();
                    var appUser = personFinded.ApplicationUser;
                    var personModel = mapper.Map<ApplicationUser, PersonDTO>(appUser);
                    return mapper.Map(personFinded, personModel);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Catch an exception in method {nameof(GetPersonById)} in class {this.GetType()}. The exception is {ex.Message}. " +
                       $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                    throw ex;

                }
            }

            public async Task<decimal> TotalBalanceOnAccounts(int id)
            {
                try
                {

                    decimal totalBalance = 0;
                    var currentPerson = await unitOfWork.PersonRepository.GetById(id);
                    if (currentPerson != null)
                    {
                        totalBalance = currentPerson.Accounts.Select(ac => ac.Balance).Sum();
                    }
                    return totalBalance;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Catch an exception in method {nameof(TotalBalanceOnAccounts)} in class {this.GetType()}. The exception is {ex.Message}. " +
                       $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                    throw ex;

                }
            }

            //Related data transaction
            public async Task<IdentityResult> UpdatePerson(PersonDTO person)
            {
                using (var trans = await unitOfWork.BeginTransaction())
                {
                    try
                    {

                        var user = await unitOfWork.UserManager.FindByIdAsync(person.ApplicationUserFkId);
                        if (user != null)
                        {
                            //user.Email = person.Email;
                            user.UserName = person.UserName;
                            user.PhoneNumber = person.PhoneNumber;
                            var personMapped = mapper.Map<Person>(person);
                            var result = await unitOfWork.UserManager.UpdateAsync(user);
                            await unitOfWork.Save();
                            personMapped.ApplicationUserFkId = user.Id;
                            unitOfWork.PersonRepository.Update(personMapped);
                            await unitOfWork.Save();
                            unitOfWork.CommitTransaction();
                            return result;
                        }
                        return null;

                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Catch an exception in method {nameof(UpdatePerson)} in class {this.GetType()}. The exception is {ex.Message}. " +
                           $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                        unitOfWork.RollbackTransaction();
                        throw ex;

                    }

                }
            }
        }
    }

