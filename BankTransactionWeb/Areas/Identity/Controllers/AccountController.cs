using AutoMapper;
using BankTransactionWeb.Areas.Identity.Models.ViewModels;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BankTransactionWeb.Areas.Identity.Controllers
{
   // [Authorize]
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

        public ActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
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
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "You must confirm your email.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The attempt to lo in was unsuccessfull.");
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


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (email == null || token == null)
            {
                return View("Error");
            }
            var result = await authService.ConfirmUserEmailAsync(email, token);
            if (result == null)
            {
                return View("Error");
            }
            if (result.Succeeded)
                return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
            else
                return View("Error");
        }

        private void AddModelErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }



        [HttpGet]
        public IActionResult Error()
        {
            return View();
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


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var person = new PersonDTO { Email = model.Email };
                var sendEmailSuccesfull = await authService.SendReserPasswordUrl(person);
                if(sendEmailSuccesfull == false)
                {
                    return View(nameof(ForgotPasswordConfirmation));
                }
                else
                {
                    return View(nameof(ForgotPasswordConfirmation));
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token = null, string email = null)
        {
            var model = new ResetPasswordViewModel()
            {
                Email = email,
                Token = token

            };
            return token == null ? View("Error") : View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var person = mapper.Map<PersonDTO>(model);
                var result = await authService.ResetPasswordForPerson(person);
                if( result==null)
                {
                    logger.LogError("The user was not found");
                    return View(nameof(ResetPasswordConfirmation));
                }
                if (result.Succeeded)
                {
                    return View(nameof(ResetPasswordConfirmation));
                }
                AddModelErrors(result);
            }
            return View(model);
        }


        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            personService.Dispose();
            authService.Dispose();
            base.Dispose(disposing);
        }
    }
}

//[HttpPost]
//[AllowAnonymous]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> Login(LoginViewModel model)
//{
//    if (ModelState.IsValid)
//    {
//        var person = mapper.Map<PersonDTO>(model);
//        var result = await authService.LoginPerson(person);
//        if (result.Succeeded)
//        {
//            logger.LogInformation("User  succesfully logged in.");
//            return RedirectToLocal(model.ReturnUrl);
//        }
//        if (result.IsLockedOut)
//        {
//            logger.LogWarning("User account locked out.");
//            return RedirectToAction(nameof(Lockout));
//        }
//        else
//        {
//            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
//            return View(model);
//        }
//    }
//    return View(model);



//}