using BankTransaction.Api.Models;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.BAL.Implementation.RestApi;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;
using BankTransactionWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly RestApiHelper restApiHelper;

        public TestController(IJwtAuthenticationService jwtAuthenticationService, JWTSecurityService securityService, RestApiHelper restApiHelper)
        {
            this.jwtAuthenticationService = jwtAuthenticationService;
            this.restApiHelper = restApiHelper;
            this.securityService = securityService;
        }
        private readonly IJwtAuthenticationService jwtAuthenticationService;
        private readonly JWTSecurityService securityService;

        static readonly string Test_EMAIL = "tyschenk40@gmail.com";
        static readonly string  Test_PASSWORD = "qWerty1_";
        static readonly string RESOURCE = "Company";
        [HttpGet("Test/CheckPolicy")]
        public async Task<IActionResult> CheckPolicy(string email=null, string password = null, string resource = null)
        {
            var authResult = await jwtAuthenticationService.LoginPerson(Test_EMAIL, Test_PASSWORD);
           
            if (String.IsNullOrEmpty(authResult.RefreshToken)|| DateTime.Now > authResult.ExpieryDate)
            {
                var refreshedResult = await RefreshBearerToken(authResult); 
                if(authResult.Success)
                {
                    authResult = refreshedResult;
                    return View(GetSomeResourse(authResult.Token, RESOURCE));
                }
                else
                    return View("Error", new ErrorViewModel() { Message = authResult.Errors.ToList() });
            }
            else if (!String.IsNullOrEmpty(authResult.RefreshToken))
            {
               
                return View(GetSomeResourse(authResult.Token, RESOURCE));
            }
            else
            {
                return View("Error", new ErrorViewModel() { Message = authResult.Errors.ToList() });

            }
        }

        private async Task<AuthResult> RefreshBearerToken(AuthResult authResult)
        {
            var refreshModel = new RefreshTokenDTO() { Token = authResult.Token, RefreshToken = authResult.RefreshToken };
            var result =await  securityService.RefreshToken(refreshModel);
            return result;
        }

        private string GetSomeResourse(string token, string resource)
        {
            var  restRequest = restApiHelper.Execute<IRestResponse>(resource, null, token, null);
            //var apiResponce = restRequest as ApiResponse;
            string message = JsonConvert.DeserializeObject<string>(restRequest.Content.ToString());
            return message;
        }


    }
}
