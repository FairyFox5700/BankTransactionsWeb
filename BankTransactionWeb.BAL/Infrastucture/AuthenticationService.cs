using BankTransaction.BAL.Abstract;
using BankTransaction.DAL.Abstract;
using BankTransaction.Entities;
using BankTransaction.Models;
using BankTransaction.Models.Mapper;
using BankTransaction.Models.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Mapper;

namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AuthenticationService> logger;
        private readonly ISender emailSender;
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUrlHelper urlHelper;


        private IUrlHelper UrlHelper => urlHelper ?? (urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext));

        public AuthenticationService(IUnitOfWork unitOfWork, ILogger<AuthenticationService> logger, ISender emailSender,
            IUrlHelperFactory urlHelperFactory,
           IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.emailSender = emailSender;
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccessor = actionContextAccessor;
            this.httpContextAccessor = httpContextAccessor;
            this.urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public async Task<IdentityUserResult> RegisterPerson(PersonDTO person)
        {

            using (unitOfWork.BeginTransaction())
            {
                try
                {
                    ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
                    if (user == null)
                    {
                        user = new ApplicationUser { Email = person.Email, UserName = person.UserName, PhoneNumber = person.PhoneNumber };
                        var result = await unitOfWork.UserManager.CreateAsync(user, person.Password);
                        await unitOfWork.Save();
                        if (result.Succeeded)
                        {
                            var token = await unitOfWork.UserManager.GenerateEmailConfirmationTokenAsync(user);
                            var confirmationLink = UrlHelper.Action("ConfirmEmail", "Account", new { token, email = user.Email }, httpContextAccessor.HttpContext.Request.Scheme);
                            var message = new CustomMessage(new List<string>() { user.Email }, "Confirmation email link", confirmationLink, null);
                            var personMapped = PersonEntityToDtoMapper.Instance.MapBack(person);
                            personMapped.ApplicationUserFkId = user.Id;
                            unitOfWork.PersonRepository.Add(personMapped);
                            await unitOfWork.Save();
                            await emailSender.SendEmailAsync(message);
                            await unitOfWork.UserManager.AddToRoleAsync(user, "User");
                            unitOfWork.CommitTransaction();
                            return IdentityUserResult.SUCCESS;
                        }
                        return IdentityUserResult.GenerateErrorResponce(result);
                    }
                    else
                    {
                        return new IdentityUserResult() { NotFound = true, Errors = new List<string>() { "Current user already registered" } };
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




        public async Task<bool> SendResetPasswordUrl(PersonDTO person)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
            if (user == null || !(await unitOfWork.UserManager.IsEmailConfirmedAsync(user)))
            {
                return false;
            }
            var token = await unitOfWork.UserManager.GeneratePasswordResetTokenAsync(user);//
            var confirmationLink = UrlHelper.Action("ResetPassword", "Account", new { token, email = user.Email }, httpContextAccessor.HttpContext.Request.Scheme);
            var message = new CustomMessage(new List<string>() { user.Email }, "Link for password reset", confirmationLink, null);

            await emailSender.SendEmailAsync(message);
            return true;
        }

        public async Task<IdentityUserResult> ResetPasswordForPerson(PersonDTO person)
        {

            var user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
            if (user == null)
            {
                return new IdentityUserResult() { NotFound = true, Errors = new List<string>() { $"User  {person.Email} not found" } };
            }
            var result = await unitOfWork.UserManager.ResetPasswordAsync(user, person.Token, person.Password);
            return result.Succeeded ? IdentityUserResult.SUCCESS : IdentityUserResult.GenerateErrorResponce(result);
        }


        public bool IsUserSignedIn(ClaimsPrincipal user)
        {
            return unitOfWork.SignInManager.IsSignedIn(user);
        }

        public async Task<IdentityUserResult> LoginPerson(PersonDTO person)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
            if (user == null)
                return new IdentityUserResult()
                {
                    Errors = new List<string>() { "User with this email does not exists." },
                    NotFound = true
                };
            if (!await unitOfWork.UserManager.IsEmailConfirmedAsync(user))
            {
                return new IdentityUserResult()
                {
                    Errors = new List<string>() { "You must confirm your email address first. Check your messages" }
                };
            }
            var result = await unitOfWork.SignInManager.PasswordSignInAsync(user.UserName, person.Password,
                person.RememberMe,
                lockoutOnFailure: true);
            if (result.Succeeded)
                return IdentityUserResult.SUCCESS;
            if (!result.Succeeded)
            {
                return new IdentityUserResult()
                {
                    Errors = new List<string>() { "Check your email and password. Login attempt is not succesful" }
                };
            }
            if (result.IsLockedOut)
            {
                return IdentityUserResult.LOCKED;
            }
            else
            {
                return new IdentityUserResult()
                {
                    Errors = new List<string>() { "Unable to login current user" }
                };
            }

        }


        public async Task SignOutPerson()
        {
            await unitOfWork.SignInManager.SignOutAsync();
        }


        public async Task<IdentityUserResult> ConfirmUserEmailAsync(string email, string code)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(email);
            if (user == null)
                return new IdentityUserResult() { Errors = new List<string>() { $"User wit email {email} not found" }, NotFound = true };
            var result = await unitOfWork.UserManager.ConfirmEmailAsync(user, code);
            return result.Succeeded ? IdentityUserResult.SUCCESS : IdentityUserResult.GenerateErrorResponce(result);
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }


    }
}
