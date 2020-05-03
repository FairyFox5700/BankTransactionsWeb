using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BankTransaction.Api.Models;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.DAL.Abstract;
using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;
using Microsoft.Extensions.Logging;

namespace BankTransaction.BAL.Implementation.RestApi
{
    public class JwtAuthenticationService:IJwtAuthenticationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJwtSecurityService jwtSecurityService;
        private readonly IMapper mapper;
        private readonly ILogger<JwtAuthenticationService> logger;

        public  JwtAuthenticationService (IUnitOfWork unitOfWork, IJwtSecurityService jwtSecurityService, IMapper mapper, ILogger<JwtAuthenticationService>logger)
        {
            this.unitOfWork = unitOfWork;
            this.jwtSecurityService = jwtSecurityService;
            this.mapper = mapper;
            this.logger = logger;
        }
         
     
        public async Task<AuthResult> LoginPerson(string email, string password)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await unitOfWork.SignInManager.PasswordSignInAsync(user.UserName, password,false,false);
                if (!result.Succeeded)
                {
                    return new AuthResult()
                    {
                        Errors = new[] { ErrorMessage.LoginAttemptNotSuccesfull.GetDescription() },
                        ErrorMessageKey = nameof(ErrorMessage.LoginAttemptNotSuccesfull)
                    };
                }
                return await jwtSecurityService.GenerateJWTToken(user.Email);
            }
            return new AuthResult()
            {
                Errors = new[] { ErrorMessage.EmailNotValid.GetDescription() },
                ErrorMessageKey = nameof(ErrorMessage.EmailNotValid)
            };
        }
        
        public async Task<AuthResult> RegisterPersonWithJwtToken(PersonDTO person)
        {
            using (unitOfWork.BeginTransaction())
            {
                try
                {
                    ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
                    if (user == null)
                    {
                        user = new ApplicationUser
                            {Email = person.Email, UserName = person.UserName, PhoneNumber = person.PhoneNumber};
                        var result = await unitOfWork.UserManager.CreateAsync(user, person.Password);
                        await unitOfWork.Save();
                        if (result.Succeeded)
                        {
                            var personMapped = mapper.Map<Person>(person);
                            personMapped.ApplicationUserFkId = user.Id;
                            unitOfWork.PersonRepository.Add(personMapped);
                            await unitOfWork.Save();
                            await unitOfWork.UserManager.AddToRoleAsync(user, "User");
                            unitOfWork.CommitTransaction();
                            return await jwtSecurityService.GenerateJWTToken(user.Email);
                        }
                        else
                        {
                            return new AuthResult
                            {
                                Errors = result.Errors.Select(x => x.Description)
                            };
                        }
                    }

                    else
                    {
                        return new AuthResult
                        {
                            Errors = new[] { ErrorMessage.EmailIsAlreadyInUse.GetDescription() },
                            ErrorMessageKey = nameof(ErrorMessage.EmailIsAlreadyInUse)
                        };
                    }
                }
                catch (Exception e)
                {
                    unitOfWork.RollbackTransaction();
                    logger.LogError(e.Message);
                    throw e;
                }
            }
        }

      
    }

   
}