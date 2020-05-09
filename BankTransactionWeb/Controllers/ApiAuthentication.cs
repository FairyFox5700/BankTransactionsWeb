using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
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

namespace BankTransaction.Web.Controllers
{
    public class ApiAuthentication : Controller
    {
        private readonly IRestApiHelper restApiHelper;
        private readonly ICookieHelperService helperService;
        private readonly IStringLocalizer<ApiResponcesShared> sharedLocalizer;

        public ApiAuthentication( IRestApiHelper restApiHelper, ICookieHelperService helperService, IStringLocalizer<ApiResponcesShared> sharedLocalizer)
        {
            this.restApiHelper = restApiHelper;
            this.helperService = helperService;
            this.sharedLocalizer = sharedLocalizer;
        }
        #region test data to remove
        static readonly string Test_EMAIL = "tyschenk40@gmail.com";
        static readonly string Test_PASSWORD = "qWerty1_";
        static readonly string RESOURCE = "Company";
        #endregion
        public async Task<IActionResult> SignIn(PersonDTO model, string returnUrl)//change to login and then mapp
        {
            model = new PersonDTO() { Email = Test_EMAIL, Password = Test_PASSWORD };
            //???????????????
            //add login service
            var authResultw = await restApiHelper.ExecuteAsync<ApiDataResponse<AuthSuccesfullModel>>("Auth/login",  body: model, Method.POST);
          
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
