using AutoMapper;
using BankTransactionWeb.Areas.Identity.Models.ViewModels;
using BankTransactionWeb.BAL.Infrastucture;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.Controllers;
using BankTransactionWeb.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankTransactionWeb.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {

        
        //private readonly UserManager<ApplicationUser> appUserManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;
        private readonly PersonService personService;
        private readonly IMapper mapper;
        public AccountController(UserManager<ApplicationUser> appUserManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger, PersonService personService, IMapper mapper)
        {
            //this.appUserManager = appUserManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.personService = personService;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
           // await personService.SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                var person = mapper.Map<PersonDTO>(model);
                var result = await personService.AddPerson(person);
                
                if (result.Succeeded)
                {
                    logger.LogInformation("User  succesfully logged in.");
                   
                    
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
               
                //var result2 = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                //if (result.Succeeded)
                //{
                //    logger.LogInformation("User  succesfully logged in.");
                //    return RedirectToLocal(returnUrl);
                //}
                //if (result.IsLockedOut)
                //{
                //    logger.LogWarning("User account locked out.");
                //    return RedirectToAction(nameof(Lockout));
                //}

                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //    return View(model);
                //}
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
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            logger.LogInformation("User successfully logged out.");
            return RedirectToAction("index", "home");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var person = mapper.Map<PersonDTO>(model);
                var result = await personService.AddPerson(person);
                if (result.Succeeded)
                {
                    logger.LogInformation("Successfully created new user.");
                    return RedirectToLocal(returnUrl);
                }
                   AddModelErrors(result);
                   // ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                //if (ModelState.IsValid)
                //{
                //    var user = new ApplicationUser
                //    {
                //        UserName = model.Email,
                //        Email = model.Email
                //    };
                //    var result = await appUserManager.CreateAsync(user, model.Password);
                //    if (result.Succeeded)
                //    {
                //        logger.LogInformation("Successfully created new user.");

                //        await signInManager.SignInAsync(user, isPersistent: false);
                //        logger.LogInformation("User signed in a new account with password.");
                //        return RedirectToLocal(returnUrl);
                //    }
                //    AddModelErrors(result);
                //}
                //return View(model);
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
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}