using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.DAL.Abstract;
using BankTransaction.Entities;
using BankTransaction.Models;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJwtSecurityService jwtSecurityService;
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

        public AuthenticationService(IUnitOfWork unitOfWork, IJwtSecurityService jwtSecurityService, IMapper mapper, ILogger<AuthenticationService> logger, ISender emailSender,
            IUrlHelperFactory urlHelperFactory, 
           IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.jwtSecurityService = jwtSecurityService;
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

            using (var trans = unitOfWork.BeginTransaction())
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
                            var confirmationLink = URLHelper.Action("ConfirmEmail", "Account", new { token, email = user.Email }, httpContextAccessor.HttpContext.Request.Scheme);
                            logger.Log(LogLevel.Warning, confirmationLink);
                            var message = new CustomMessage(new List<string>() { user.Email }, "Confirmation email link", confirmationLink, null);
                            var personMapped = mapper.Map<Person>(person);
                            personMapped.ApplicationUserFkId = user.Id;
                            unitOfWork.PersonRepository.Add(personMapped);
                            await unitOfWork.Save();
                            await emailSender.SendEmailAsync(message);
                            await unitOfWork.UserManager.AddToRoleAsync(user, "User");
                            unitOfWork.CommitTransaction();
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
                    unitOfWork.RollbackTransaction();
                    throw ex;
                }
            }
        }


        public async Task<AuthResult> RegisterPersonWithJwtToken(PersonDTO person)
        {
            using (var trans = unitOfWork.BeginTransaction())
            {

                ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
                if (user == null)
                {
                    user = new ApplicationUser { Email = person.Email, UserName = person.UserName, PhoneNumber = person.PhoneNumber };
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
                        Errors = new[] { "User with this email is alredy registered" }
                    };
                }
            }
        }

        public async Task<bool> SendReserPasswordUrl(PersonDTO person)
        {
            try
            {
                var user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
                if (user == null || !(await unitOfWork.UserManager.IsEmailConfirmedAsync(user)))
                {
                    return false;
                }
                var token = await unitOfWork.UserManager.GeneratePasswordResetTokenAsync(user);//
                var confirmationLink = URLHelper.Action("ResetPassword", "Account", new { token, email = user.Email }, httpContextAccessor.HttpContext.Request.Scheme);
                var message = new CustomMessage(new List<string>() { user.Email }, "Link for password reset", confirmationLink, null);

                await emailSender.SendEmailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IdentityResult> ResetPasswordForPerson(PersonDTO person)
        {
            try
            {

                var user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
                if (user == null)
                {
                    return null;
                }
                var result = await unitOfWork.UserManager.ResetPasswordAsync(user, person.Token, person.Password);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool IsUserSignedIn(ClaimsPrincipal user)
        {
            try
            {
                if (unitOfWork.SignInManager.IsSignedIn(user))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<AuthResult> LoginPerson(PersonDTO person)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(person.Email);
            if (user != null)
            {
                if (!await unitOfWork.UserManager.IsEmailConfirmedAsync(user))
                {
                    return new AuthResult()
                    {
                        Errors = new[] { "You must confirm your email adrees first. Check your messages" }
                    };
                }
                var result = await unitOfWork.SignInManager.PasswordSignInAsync(user.UserName, person.Password,
                    person.RememberMe,
                    lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    return new AuthResult()
                    {
                        Errors = new[] { "Check your email and password. Login attemp is not succesfull" }
                    };
                }
                return await jwtSecurityService.GenerateJWTToken(user.Email);
            }
            return new AuthResult()
            {
                Errors = new[] { "User with this email does not exists." }
            };
        }


       


        public async Task SignOutPerson()
        {
            try
            {
                await unitOfWork.SignInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public async Task<IdentityResult> ConfirmUserEmailAsync(string email, string code)
        {
            try
            {
                var user = await unitOfWork.UserManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return null;
                }
                var result = await unitOfWork.UserManager.ConfirmEmailAsync(user, code);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


      

           

            public void Dispose()
            {
                unitOfWork.Dispose();
            }

            Task IAuthenticationService.RefreshToken(RefreshTokenDTO model)
        {
                throw new NotImplementedException();
            }
        }
    }
