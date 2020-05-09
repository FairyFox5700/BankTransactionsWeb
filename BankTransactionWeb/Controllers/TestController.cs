using BankTransaction.Api.Models;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.BAL.Implementation.RestApi;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;
using BankTransaction.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly IRestApiHelper restApiHelper;
        private readonly IStringLocalizer<ApiResponcesShared> sharedLocalizer;

        public TestController(IJwtAuthenticationService jwtAuthenticationService, JWTSecurityService securityService, IRestApiHelper restApiHelper,IStringLocalizer<ApiResponcesShared> sharedLocalizer)
        {
            this.jwtAuthenticationService = jwtAuthenticationService;
            this.restApiHelper = restApiHelper;
            this.sharedLocalizer = sharedLocalizer;
            this.securityService = securityService;
        }
        private readonly IJwtAuthenticationService jwtAuthenticationService;
        private readonly JWTSecurityService securityService;

        static readonly string Test_EMAIL = "tyschenk40@gmail.com";
        static readonly string  Test_PASSWORD = "qWerty1_";
        static readonly string RESOURCE = "Company";
        [HttpGet("Test/CheckPolicy")]
        //string message = _sharedLocalizer["Message"];
        public async Task<IActionResult> CheckPolicy(string email=null, string password = null, string resource = null)
        {
            var authResult = await jwtAuthenticationService.LoginPerson(new PersonDTO(){ Email = Test_EMAIL, Password = Test_PASSWORD });
           
            if (String.IsNullOrEmpty(authResult.RefreshToken)|| DateTime.Now > authResult.ExpieryDate)
            {
                var refreshedResult = await RefreshBearerToken(authResult); 
                if(authResult.Success)
                {
                    authResult = refreshedResult;
                    return View(GetSomeResourse(authResult.Token, RESOURCE));
                }
                else
                    return View("Error", new ErrorViewModel() { Message =JsonConvert.DeserializeObject<List<string>>( authResult.Message) });
            }
            else if (!String.IsNullOrEmpty(authResult.RefreshToken))
            {
               
                return View(GetSomeResourse(authResult.Token, RESOURCE));
            }
            else
            {
                return View("Error", new ErrorViewModel() { Message = JsonConvert.DeserializeObject<List<string>>(authResult.Message) });
            }
        }

        private async Task<AuthResult> RefreshBearerToken(AuthResult authResult)
        {
            var refreshModel = new RefreshTokenDTO() { Token = authResult.Token, RefreshToken = authResult.RefreshToken };
            var result =await  securityService.RefreshToken(refreshModel);
            return result;
        }

        private ApiDataResponse<List<CompanyDTO>> GetSomeResourse(string token, string resource)
        {
            var  restResponce = restApiHelper.Execute<ApiDataResponse<List<CompanyDTO>>>(resource, null,  Method.GET,null);
            //var jsonResponse = JsonConvert.DeserializeObject<ApiDataResponse<List<CompanyDTO>>>(restResponce);
            return restResponce;
        }
        //                    new JsonSerializerSettings
    //                {
    //                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
    //}


}
}
