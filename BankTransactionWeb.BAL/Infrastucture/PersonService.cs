using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Identity;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
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
        //public async Task AddPerson(PersonDTO person)
        //{
        //    try
        //    {


        //            var personMapped = mapper.Map<Person>(person);
        //            unitOfWork.PersonRepository.Add(personMapped);
        //            await unitOfWork.Save();
        //            logger.LogInformation($"In method {nameof(AddPerson)} instance of person successfully added");

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError($"Catch an exception in method {nameof(AddPerson)} in class {this.GetType()}. The exception is {ex.Message}. " +
        //           $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
        //        throw ex;

        //    }

        //}
        public async Task<IdentityResult> AddPerson(PersonDTO person)
        {

            ApplicationUser user = await unitOfWork.AppUserManager.FindByEmailAsync(person.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = person.Email, UserName = person.UserName };
                var result = await unitOfWork.AppUserManager.CreateAsync(user, person.Password);
                if (result.Errors.Count() > 0)
                    return result;
                await unitOfWork.AppUserManager.AddToRoleAsync(user, person.Role);
                var personMapped = mapper.Map<Person>(person);
                personMapped.AplicationUserId = user.Id;
                unitOfWork.PersonRepository.Add(personMapped);
                await unitOfWork.Save();
                return result;

            }
            else
            {
                var identRes = new IdentityResult();
                var identityError = new IdentityError();
                identityError.Code = "InvaliEmail";
                identityError.Description = "Already exisst person with this email";
                identRes.Errors.Append(identityError);
                return identRes;
            }
        }

        public async Task SetInitialData( PersonDTO person, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await unitOfWork.AppRoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole() { Name = roleName };
                    await unitOfWork.AppRoleManager.CreateAsync(role);
                }
            }
            await AddPerson(person);
        }

        //public async Task<ClaimsIdentity> Authenticate(PersonDTO person)
        //{
        //    ClaimsIdentity claim = null;
        //    ApplicationUser user = (await unitOfWork.PersonRepository.GetById(person.Id)).ApplicationUser;
        //    if(user!=null)
        //    {
        //        var factory = new UserClaimsPrincipalFactory<ApplicationUser>(unitOfWork.AppUserManager,null);
        //        ClaimsPrincipal claimsPrincipal = await factory.CreateAsync(user,this);
        //        ((ClaimsIdentity)claimsPrincipal.Identity).AddClaim(new Claim("user_id", user.Id.ToString()));
        //        claim = await unitOfWork.AppUserManager.AddClaimAsync(user, ClaimValueTypes.Cook)
        //    }
        //}

        //var identityResult = await unitOfWork.AppUserManager.CreateAsync(user, person.Password);
        //if(identityResult.Succeeded)
        //{
        //   await unitOfWork.ApplicationSignInManager.SignInAsync(user, isPersistent: false);

        //}
        //return identityResult;
        //return new AddPersonResponse(user.Id, identityResult.Succeeded, 
        // identityResult.Succeeded ? null : identityResult.Errors.Select(er => new Error(er.Code, er.Description)).ToList());

        //public async Task<ServiceOperationDetails> AddPerson(PersonDTO person)
        //{
        //    try
        //    {
        //        ApplicationUser user = await unitOfWork.AppUserManager.FindByEmailAsync(person.Email);
        //        if (user == null)
        //        {
        //            user = new ApplicationUser { Email = person.Email, UserName = person.UserName };
        //            var result = await unitOfWork.AppUserManager.CreateAsync(user, person.Password);
        //            if (result.Errors.Count() > 0)
        //            {
        //                return new ServiceOperationDetails(false, result.Errors.FirstOrDefault()?.ToString(), "");
        //            }
        //            await unitOfWork.AppUserManager.AddToRoleAsync(user, person.Role);

        //            var personMapped = mapper.Map<Person>(person);
        //            personMapped.AplicationUserId = user.Id;
        //            unitOfWork.PersonRepository.Add(personMapped);
        //            await unitOfWork.Save();
        //            logger.LogInformation($"In method {nameof(AddPerson)} instance of person successfully added");
        //            return new ServiceOperationDetails(true, "Resistration was succcessfull", "");
        //        }
        //        else
        //        {
        //            return new ServiceOperationDetails(true, "There is somebode registered with this email.", "");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError($"Catch an exception in method {nameof(AddPerson)} in class {this.GetType()}. The exception is {ex.Message}. " +
        //           $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
        //        throw ex;

        //    }
        //}

        public async Task DeletePerson(PersonDTO person)
        {
            try
            {
                var personMapped = mapper.Map<Person>(person);
                unitOfWork.PersonRepository.Delete(personMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(DeletePerson)} instance of person successfully added");
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeletePerson)} in class {nameof(PersonService)}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

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

                return mapper.Map<PersonDTO>(personFinded);
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

        public async Task UpdatePerson(PersonDTO person)
        {
            try
            {
                var personMapped = mapper.Map<Person>(person);
                unitOfWork.PersonRepository.Update(personMapped);
                await unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdatePerson)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }
    }
}
