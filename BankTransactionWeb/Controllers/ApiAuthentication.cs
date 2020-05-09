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
        private readonly UrlsConfiguration urlsConfiguration;
        private readonly IRestApiHelper restApiHelper;
        private readonly ICookieHelperService helperService;
        private readonly IStringLocalizer<ApiResponcesShared> sharedLocalizer;

        public ApiAuthentication(UrlsConfiguration urlsConfiguration, IRestApiHelper restApiHelper, ICookieHelperService helperService, IStringLocalizer<ApiResponcesShared> sharedLocalizer)
        {
            this.urlsConfiguration = urlsConfiguration;
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
            var authResultw = await restApiHelper.ExecuteAsync<ApiResponse<AuthSuccesfullModel>>("Auth/login",  body: model, Method.POST);
            //helperService.AddReplaceCookie("BankWeb.AspNetCore.ProductKey", authResultw.Data.Token);
            Response.Cookies.Append(
               "BankWeb.AspNetCore.ProductKey", authResultw.Data.Token,
              new CookieOptions
              {
                  MaxAge = TimeSpan.FromMinutes(60),
                  IsEssential = true
              });
            //         HttpContext.Response.Cookies.Append("BankWeb.AspNetCore.ProductKey", authResultw.Data.Token,
            //new CookieOptions
            //{
            //    MaxAge = TimeSpan.FromMinutes(60),
            //    IsEssential = true
            //});
            // var authResult = await jwtAuthenticationService.LoginPerson(new PersonDTO() { Email = Test_EMAIL, Password = Test_PASSWORD });
            //if (String.IsNullOrEmpty(authResult.Token) || DateTime.Now > authResult.ExpieryDate)
            //{
            //    HttpContext.Response.Cookies.Append("BankWeb.AspNetCore.ProductKey", authResult.Token,
            //     new CookieOptions
            //     {
            //         MaxAge = TimeSpan.FromMinutes(60)
            //     });
            //}
            //var claims = new List<Claim>
            //{
            // new Claim(ClaimTypes.Name, Guid.NewGuid().ToString())
            //};

            //var claimsIdentity = new ClaimsIdentity(
            //  claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var authProperties = new AuthenticationProperties
            //{
            //    ExpiresUtc = DateTime.UtcNow.AddYears(1),
            //    IsPersistent = true
            //};
            //await HttpContext.SignInAsync(
            //  CookieAuthenticationDefaults.AuthenticationScheme,
            //  new ClaimsPrincipal(claimsIdentity),
            //  authProperties);
            //var user = User as ClaimsPrincipal;
            //var token = await HttpContext.GetTokenAsync("access_token");
            //if (token != null)
            //{
            //    ViewData["access_token"] = token;
            //}
            //var authticationProperties = new AuthenticationProperties();
            //await HttpContext.SignInAsync(
            // CookieAuthenticationDefaults.AuthenticationScheme, user, authticationProperties);
            return RedirectToAction("Index", "Home");
        }
        [Authorize(AuthenticationSchemes = "Bearer,Cookies")]
        [HttpPost]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
