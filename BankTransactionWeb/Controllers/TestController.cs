using System;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.BAL.Implementation.RestApi;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace BankTransaction.Web.Controllers
{
    public class TestController : Controller
    {
        private static readonly string Test_EMAIL = "tyschenk40@gmail.com";
        private static readonly string Test_PASSWORD = "qWerty1_";
        private static readonly string RESOURCE = "Company";
        private readonly IJwtAuthenticationService jwtAuthenticationService;
        private readonly RestApiHelper restApiHelper;
        private readonly JWTSecurityService securityService;

        public TestController(IJwtAuthenticationService jwtAuthenticationService, JWTSecurityService securityService,
            RestApiHelper restApiHelper)
        {
            this.jwtAuthenticationService = jwtAuthenticationService;
            this.restApiHelper = restApiHelper;
            this.securityService = securityService;
        }

        [HttpGet("Test/CheckPolicy")]
        public async Task<IActionResult> CheckPolicy(string email = null, string password = null,
            string resource = null)
        {
            var authResult = await jwtAuthenticationService.LoginPerson(Test_EMAIL, Test_PASSWORD);

            if (string.IsNullOrEmpty(authResult.RefreshToken) || DateTime.Now > authResult.ExpieryDate)
            {
                var refreshedResult = await RefreshBearerToken(authResult);
                if (authResult.Success)
                {
                    authResult = refreshedResult;
                    return View(GetSomeResourse(authResult.Token, RESOURCE));
                }

                return View("Error", new ErrorViewModel {Message = authResult.Errors.ToList()});
            }

            if (!string.IsNullOrEmpty(authResult.RefreshToken))
                return View(GetSomeResourse(authResult.Token, RESOURCE));
            return View("Error", new ErrorViewModel {Message = authResult.Errors.ToList()});
        }

        private async Task<AuthResult> RefreshBearerToken(AuthResult authResult)
        {
            var refreshModel = new RefreshTokenDTO {Token = authResult.Token, RefreshToken = authResult.RefreshToken};
            var result = await securityService.RefreshToken(refreshModel);
            return result;
        }

        private string GetSomeResourse(string token, string resource)
        {
            var restRequest = restApiHelper.Execute<IRestResponse>(resource, null, token);
            //var apiResponce = restRequest as ApiResponse;
            var message = JsonConvert.DeserializeObject<string>(restRequest.Content);
            return message;
        }
    }
}