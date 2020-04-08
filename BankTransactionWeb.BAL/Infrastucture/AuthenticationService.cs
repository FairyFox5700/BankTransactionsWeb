using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Infrastucture
{
   public class AuthenticationService: IAuthenticationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<AuthenticationService> logger;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthenticationService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IdentityResult> RegisterPerson(PersonDTO person)
        {
            ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = person.Email, UserName = person.UserName };
                var result = await unitOfWork.UserManager.CreateAsync(user, person.Password);
                if (result.Errors.Count() > 0)
                    return result;
                var personMapped = mapper.Map<Person>(person);
                personMapped.ApplicationUserId = user.Id;
                unitOfWork.PersonRepository.Add(personMapped);
                await unitOfWork.Save();
                await SignInPerson(user);
                return result;
            }
            else
            {
                return null;
            }
        }

        public async Task<SignInResult> LoginPerson(PersonDTO person)
        {
            var result = await unitOfWork.SignInManager.PasswordSignInAsync(person.UserName, person.Password, person.RememberMe, lockoutOnFailure: true);
            return result;
        }

        public async Task SignOutPerson()
        {
            await unitOfWork.SignInManager.SignOutAsync();
        }

        private async Task SignInPerson(ApplicationUser user)
        {
            await unitOfWork.SignInManager.SignInAsync(user, isPersistent: false);
        }

    }
}
