using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.DAL.Abstract;
using BankTransaction.Entities;
using BankTransaction.Entities.Filter;
using BankTransaction.Models;
using BankTransaction.Models.Mapper;
using BankTransaction.Models.Mapper.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<PersonService> logger;

        public PersonService(IUnitOfWork unitOfWork,  ILogger<PersonService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task AddPerson(PersonDTO person)
        {
           
                var personMapped = PersonEntityToDtoMapper.Instance.MapBack(person);
                unitOfWork.PersonRepository.Add(personMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(AddPerson)} instance of person successfully added");
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
                        var personMapped = PersonEntityToDtoMapper.Instance.MapBack(person);
                        unitOfWork.PersonRepository.Delete(personMapped);
                        await unitOfWork.Save();
                        var result = await unitOfWork.UserManager.DeleteAsync(user);
                        unitOfWork.CommitTransaction();
                        logger.LogInformation($"In method {nameof(DeletePerson)} instance of person successfully deleted");
                        return result;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    unitOfWork.RollbackTransaction();
                    throw ex;

                }
            }
        }


        public void Dispose()
        {
            unitOfWork.Dispose();
        }
        public async Task<PaginatedModel<PersonDTO>> GetAllPersons(int pageNumber, int pageSize, PersonFilterModel personFilter = null)
        {
                PaginatedPlainModel<Person> persons =null;
                   
                if (personFilter != null)
                {
                    var filter = PersonFilterDtoToPerson.Instance.Map(personFilter);
                    persons = await unitOfWork.PersonRepository.GetAll(pageNumber,pageSize,filter);
                }
                else
                {
                    persons = await unitOfWork.PersonRepository.GetAll(pageNumber, pageSize);
                }
                //TODo smt better here
                return new PaginatedModel<PersonDTO>(persons.Select(p => PersonEntityToDtoMapper.Instance.Map(p)),persons.PageNumber, persons.PageSize,persons.TotalCount, persons.TotalPages);
          

        }



        public async Task<PersonDTO> GetPersonById(int id)
        {
            try
            {
                var personFinded = await unitOfWork.PersonRepository.GetById(id);
                var appUser = personFinded.ApplicationUser;
                var personModel = ApplicationUserEntityToPersonDtoMapper.Instance.Map(appUser);
                personModel.LastName = personFinded.LastName;
                personModel.Name = personFinded.Name;
                personModel.Surname = personFinded.Surname;
                return personModel;
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
                var personFinded = await unitOfWork.PersonRepository.GetPersonByAccount(id);
                var appUser = personFinded.ApplicationUser;
                var personModel = ApplicationUserEntityToPersonDtoMapper.Instance.Map(appUser);
                personModel.LastName = personFinded.LastName;
                personModel.Name = personFinded.Name;
                personModel.Surname = personFinded.Surname;
                return personModel;
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
                        var personMapped = PersonEntityToDtoMapper.Instance.MapBack(person);
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

