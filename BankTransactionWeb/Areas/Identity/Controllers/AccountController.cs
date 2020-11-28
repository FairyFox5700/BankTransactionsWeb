using BankTransaction.BAL.Abstract;
using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;
using BankTransaction.Web.Mapper.Identity;

namespace BankTransaction.Web.Areas.Identity.Controllers
{
    [Authorize]
    [Area("Identity")]
    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> logger;
        private readonly IPersonService personService;
        private readonly IAuthenticationService authService;

        public AccountController(ILogger<AccountController> logger, IPersonService personService,  IAuthenticationService authService)
        {
            this.logger = logger;
            this.personService = personService;
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

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }


        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var person = LoginToPersonDtoMapper.Instance.Map(model);
                var result = await authService.LoginPerson(person);
                if (result.NotFound)
                    return NotFound(result);
                if (result.Locked)
                    return RedirectToAction(nameof(Lockout));
                if (result.Succeeded)
                {
                    return RedirectToLocal(model.ReturnUrl);
                }
                else
                {
                    AddModelErrors(result);
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return Ok();
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
                var person = RegisterToPersonDtoMapper.Instance.Map(model);
                var result = await authService.RegisterPerson(person);
                if (result.NotFound)
                    return NotFound(result.Errors);
                if (result.Locked)
                    return RedirectToAction(nameof(Lockout));
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    AddModelErrors(result); 
                    return View(model);
                }
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
            if (result.NotFound)
            {
                return NotFound((result));
            }
            else if (result.Succeeded)
                return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
            else
            //TODO Error
                return View("Error",result);
        }

        private void AddModelErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        private void AddModelErrors(IdentityUserResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }


        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// Method returns user after succesfull login or registration.
        /// Avoid redirect from malicious website
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
            try
            {
                if (ModelState.IsValid)
                {
                    var person = new PersonDTO { Email = model.Email };
                    var sendEmailSuccesfull = await authService.SendResetPasswordUrl(person);
                    if (sendEmailSuccesfull == false)
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
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(ForgotPassword)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }

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
                var person = ResetPasswordToPersonDtoMapper.Instance.Map(model);
                var result = await authService.ResetPasswordForPerson(person);
                if (result.NotFound)
                {
                    return View(nameof(ResetPasswordConfirmation));
                }
                else if (result.Succeeded)
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

