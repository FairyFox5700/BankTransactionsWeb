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
    public class UrlsConfiguration
    {
        public class AccountOperations
        {
            public static string GetItemsById(IEnumerable<int> ids) => $"/api/v1/catalog/items?ids={string.Join(',', ids)}";
            public static string GetOrderDraft() => "/api/v1/orders/draft";
            public static string GetAllAcoounts() => "account";
        }
        public string Account  { get; set; } = "account";
        public  string DeleteAccount { get; set; } = "account/delete";
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
