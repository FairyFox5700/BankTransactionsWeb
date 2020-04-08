using AutoMapper;
using BankTransactionWeb.Areas.Identity.Models.ViewModels;
using BankTransactionWeb.BAL.Infrastucture;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.Controllers;
using BankTransactionWeb.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankTransactionWeb.Areas.Identity.Controllers
{
    [Authorize]
    [Area("Identity")]
    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> logger;
        private readonly IPersonService personService;
        private readonly IMapper mapper;
        private readonly IAuthenticationService authService;

        public AccountController(ILogger<AccountController> logger, IPersonService personService, IMapper mapper, IAuthenticationService authService)
        {
            this.logger = logger;
            this.personService = personService;
            this.mapper = mapper;
            this.authService = authService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        
        public ActionResult Login(string returnUrl =null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var person = mapper.Map<PersonDTO>(model);
                var result = await authService.LoginPerson(person);
                if (result.Succeeded)
                {
                    logger.LogInformation("User  succesfully logged in.");
                    return RedirectToLocal(model.ReturnUrl);
                }
                if (result.IsLockedOut)
                {
                    logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
           

         
        }

        [HttpGet]
        [AllowAnonymous]
        private object Lockout()
        {
             return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await authService.SignOutPerson();
            logger.LogInformation("User successfully logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            
            if (ModelState.IsValid)
            {
                var person = mapper.Map<PersonDTO>(model);
                var result = await authService.RegisterPerson(person);
                if (result == null)
                {
                    ModelState.AddModelError("RegiterFailed", "There is alreasy user with this login");
                    return View(model);
                }
                if (result.Succeeded)
                {
                    logger.LogInformation("Successfully created new user.");
                    logger.LogInformation("User signed in a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
               
                AddModelErrors(result);
            }
            return View(model);

        }

        private void AddModelErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        /// <summary>
        /// Method returns user after succesfull lodin or registration.
        /// Avoid redirect from malicios website
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
            }
        }
    }
}