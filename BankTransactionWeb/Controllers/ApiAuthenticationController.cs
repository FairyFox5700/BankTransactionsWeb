using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.Configuration;
using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.Web.Mapper.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.Localization;

namespace BankTransaction.Web.Controllers
{
    public class ApiAuthenticationController : Controller
    {
        private readonly IRestApiHelper restApiHelper;
        private readonly ICookieHelperService helperService;
        private readonly IStringLocalizer<ApiResponcesShared> sharedLocalizer;

        public ApiAuthenticationController( IRestApiHelper restApiHelper, ICookieHelperService helperService, IStringLocalizer<ApiResponcesShared> sharedLocalizer)
        {
            this.restApiHelper = restApiHelper;
            this.helperService = helperService;
            this.sharedLocalizer = sharedLocalizer;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        #region test data to remove
        static readonly string Test_EMAIL = "tyschenk40@gmail.com";
        static readonly string Test_PASSWORD = "qWerty1_";
        static readonly string RESOURCE = "Company";
        #endregion
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginViewModel model)//change to login and then mapp
        {
            var person = LoginToPersonDtoMapper.Instance.Map(model);
            person = new PersonDTO() { Email = Test_EMAIL, Password = Test_PASSWORD };
            //???????????????
            //add login service
            var authResultw = await restApiHelper.ExecuteAsync<ApiDataResponse<AuthSuccesfullModel>>("Auth/login",  body: person, Method.POST);
            Response.Cookies.Append("wwwwww", "2312213");
            HttpContext.Response.Cookies.Append("BankWeb.AspNetCore.ProductKey", authResultw.Data.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    MaxAge = TimeSpan.FromMinutes(60)
                });
            HttpContext.Response.Cookies.Append("BankWeb.AspNetCore.ProductKeyFree", authResultw.Data.RefreshToken,
           new CookieOptions
           {
               HttpOnly = true,
               MaxAge = TimeSpan.FromMinutes(60)
           });
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
