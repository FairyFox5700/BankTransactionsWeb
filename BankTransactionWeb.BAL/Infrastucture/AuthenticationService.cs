using AutoMapper;
using BankTransactionWeb.BAL.Infrastucture.MessageServices;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<AuthenticationService> logger;
        private readonly ISender emailSender;
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUrlHelper urlHelper;
        public IUrlHelper URLHelper
        {
            get => urlHelper ?? (urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext));
        }

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthenticationService> logger, ISender emailSender,
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
            this.urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public async Task<IdentityResult> RegisterPerson(PersonDTO person)
        {
            try
            {

                ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
                if (user == null)
                {
                    user = new ApplicationUser { Email = person.Email, UserName = person.UserName };
                    var result = await unitOfWork.UserManager.CreateAsync(user, person.Password);
                    if (result.Succeeded)
                    {
                        var token = await unitOfWork.UserManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = URLHelper.Action("ConfirmEmai", "Account", new { token, email = user.Email }, httpContextAccessor.HttpContext.Request.Scheme);
                        logger.Log(LogLevel.Warning, confirmationLink);
                        var message = new CustomMessage(new List<string>() { user.Email }, "Confirmation email link", confirmationLink, null);
                        var personMapped = mapper.Map<Person>(person);
                        personMapped.ApplicationUserFkId = user.Id;
                        unitOfWork.PersonRepository.Add(personMapped);
                        await unitOfWork.Save();
                        await emailSender.SendEmailAsync(message);
                        //await unitOfWork.UserManager.AddToRoleAsync(user, "Visitor");
                        return result;
                    }
                    return result;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"An exceprion {ex} occured on registering new user. Inner exeprion {ex.InnerException}");
                throw ex;
            }
        }

        public async Task<bool> SendReserPasswordUrl(PersonDTO person)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
            if (user == null || !(await unitOfWork.UserManager.IsEmailConfirmedAsync(user)))
            {
                return false;
            }
            var token = await unitOfWork.UserManager.GeneratePasswordResetTokenAsync(user);
            var confirmationLink = URLHelper.Action("ConfirmEmai", "Account", new { token, email = user.Email }, httpContextAccessor.HttpContext.Request.Scheme);
            var message = new CustomMessage(new List<string>() { user.Email }, "Link for password reset", confirmationLink, null);

            await emailSender.SendEmailAsync(message);
            return true;
        }

        public async Task<IdentityResult> ResetPasswordForPerson(PersonDTO person)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
            if (user == null)
            {
                return null;
            }
            var result = await unitOfWork.UserManager.ResetPasswordAsync(user, person.Code, person.Password);
            return result;
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

        //private async Task SignInPerson(ApplicationUser user)
        //{
        //    await unitOfWork.SignInManager.SignInAsync(user, isPersistent: false);
        //}


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