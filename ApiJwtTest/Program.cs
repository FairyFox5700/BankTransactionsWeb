using BankTransaction.Api.Models;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.BAL.Implementation.RestApi;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ApiJwtTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
        private readonly IJwtAuthenticationService jwtAuthenticationService;

      
        public async Task CheckPolicy(string email, string password, string resource)
        {
            var authResult = await jwtAuthenticationService.LoginPerson(email, password);
            //if (authResult.Token != null && DateTime.Now < authResult.ExpieryDate)
            //{
            //    //use the existing token
            //}
            //else if (authResult.Token != null && !String.IsNullOrEmpty(authResult.RefreshToken))
            //{
            //    //Get a new access token based on the Refresh Token
            //    token = GetTokens(_clientId, _clientSecret, token.RefreshToken);
            //}
            if (!String.IsNullOrEmpty(authResult.RefreshToken))
            {
                Console.WriteLine( GetSomeResourse(authResult.Token, resource));
            }
            else
            {
                //TODO maybe error view
                Console.WriteLine(authResult.Errors);
            }
        }

        private string GetSomeResourse(string token, string resource)
        {
            var currenResorce = "/person";
            //TODO not interace
            var restHelper = new RestApiHelper();
            var restRequest = restHelper.ExecuteApiRequest<ApiResponse>(currenResorce, token, null);
            var apiResponce = restRequest as ApiResponse;
            var kqweq = apiResponce.ToString();
            string message = JsonConvert.DeserializeObject<string>(kqweq);
            return message;
        }
    }
}
