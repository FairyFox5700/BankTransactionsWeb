using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankTransaction.Configuration
{
    public static class API
    {
        public static class Account
        {
            public static string Acounts => "account";
            public static string UpdateAccount(int id) => $"account/{id}";
            public static string DeleteAccount(int id) => $"account/{id}";
            //public static string DeleteAccount => "account/delete";
        }
        public static class Authorization
        {
            public static string Login => "Auth/login";
            public static string Logout => "Auth/token/cancel";
            public static string Refresh => "Auth/refreshToken";
        }

       
    }

    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler

    {//In Startup
     //https://github.com/dotnet-architecture/eShopOnContainers/blob/43fe719e98bb7e004c697d5724a975f5ecb2191b/src/Web/WebMVC/Startup.cs
     //implementation
     //https://github.com/dotnet-architecture/eShopOnContainers/blob/1b7200791931f33c94206822a69644ca820bb0dc/src/ApiGateways/Web.Bff.Shopping/aggregator/Infrastructure/HttpClientAuthorizationDelegatingHandler.cs

        private readonly IHttpContextAccessor _httpContextAccesor;

        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccesor = httpContextAccesor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _httpContextAccesor.HttpContext
                .Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }

            var token = await GetToken();

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        async Task<string> GetToken()
        {
            const string ACCESS_TOKEN = "access_token";

            return await _httpContextAccesor.HttpContext
                .GetTokenAsync(ACCESS_TOKEN);
        }
    }



}
