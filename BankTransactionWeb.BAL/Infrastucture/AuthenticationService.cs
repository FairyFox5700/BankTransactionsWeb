using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;
using BankTransactionWeb.BAL.Infrastucture.MessageServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<AuthenticationService> logger;
        private readonly EmailSender emailSender;
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthenticationService> logger, EmailSender emailSender,
            IUrlHelperFactory urlHelperFactory,
           IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.emailSender = emailSender;
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccessor = actionContextAccessor;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IdentityResult> RegisterPerson(PersonDTO person)
        {
            ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = person.Email, UserName = person.UserName };
                var result = await unitOfWork.UserManager.CreateAsync(user, person.Password);
                if (result.Succeeded)
                {
                    var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
                    var token = await unitOfWork.UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = urlHelper.Action("ConfirmEmai", "Account", new { token, email = user.Email }, httpContextAccessor.HttpContext.Request.Scheme);
                    logger.Log(LogLevel.Warning, confirmationLink);
                    var message = new CustomMessage(new List<string>() { user.Email }, "Confirmation email link", confirmationLink, null);
                    var personMapped = mapper.Map<Person>(person);
                    personMapped.ApplicationUserId = user.Id;
                    unitOfWork.PersonRepository.Add(personMapped);
                    await unitOfWork.Save();
                    await emailSender.SendEmailAsync(message);
                    return result;
                }
                return result;
            }
            else
            {
                return null;
            }
        }



        public async Task<SignInResult> LoginPerson(PersonDTO person)
        {
            var user = await unitOfWork.UserManager.FindByNameAsync(person.UserName);
            if (user != null)
            {
                if (!await unitOfWork.UserManager.IsEmailConfirmedAsync(user))
                {
                    return null;
                }
            }
            var result = await unitOfWork.SignInManager.PasswordSignInAsync(person.UserName, person.Password, person.RememberMe, lockoutOnFailure: true);
            return result;
        }


        //public async Task<SignInResult> LoginPerson(PersonDTO person)
        //{
        //    var result = await unitOfWork.SignInManager.PasswordSignInAsync(person.UserName, person.Password, person.RememberMe, lockoutOnFailure: true);
        //    return result;
        //}

        public async Task SignOutPerson()
        {
            await unitOfWork.SignInManager.SignOutAsync();
        }

        private async Task SignInPerson(ApplicationUser user)
        {
            await unitOfWork.SignInManager.SignInAsync(user, isPersistent: false);
        }


        public async Task<IdentityResult> ConfirmUserEmailAsync(string userId, string code)
        {
            var user = await unitOfWork.UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            var result = await unitOfWork.UserManager.ConfirmEmailAsync(user, code);
            return result;
        }
    }
}

        //public async Task<IdentityResult> RegisterPerson(PersonDTO person)
        //{
        //    ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
        //    if (user == null)
        //    {
        //        user = new ApplicationUser { Email = person.Email, UserName = person.UserName };
        //        var result = await unitOfWork.UserManager.CreateAsync(user, person.Password);
        //        if (result.Succeeded)
        //        {

        //            var personMapped = mapper.Map<Person>(person);
        //            personMapped.ApplicationUserId = user.Id;
        //            unitOfWork.PersonRepository.Add(personMapped);
        //            await unitOfWork.Save();
        //            await SignInPerson(user);
        //            return result;
        //        }
        //    }

        //    else
        //    {
        //        return null;
        //    }
        //}
        //public async Task<SignInResult> LoginPerson(PersonDTO person)
        //{
        //    var result = await unitOfWork.SignInManager.PasswordSignInAsync(person.UserName, person.Password, person.RememberMe, lockoutOnFailure: true);
        //    return result;
        //}