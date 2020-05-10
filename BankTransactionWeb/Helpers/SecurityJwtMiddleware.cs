using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract.RestApi;
using Microsoft.AspNetCore.Http;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankTransaction.Web.Helpers
{
    public class SecurityJwtMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IRestApiHelper restApiHelper;

        public SecurityJwtMiddleware(RequestDelegate next, IRestApiHelper restApiHelper)
        {
            this.next = next;
            this.restApiHelper = restApiHelper;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["BankWeb.AspNetCore.ProductKey"];
            var refreshtoken = context.Request.Cookies["BankWeb.AspNetCore.ProductKeyFree"];
            if (!string.IsNullOrEmpty(token))
                context.Request.Headers.Add("Authorization", "Bearer " + token);
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Xss-Protection", "1");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            //await this.ProcessRequestAsync(token, refreshtoken, context).ConfigureAwait(false);
            await next(context);
        }
        private async Task ProcessRequestAsync(string token, string refreshtoken, HttpContext context)
        {
            var refeshedValue = await restApiHelper.ExecuteAsync<ApiDataResponse<AuthSuccesfullModel>>("Auth/refreshToken", body: new { Token = token, RefreshToken = refreshtoken }, Method.POST, null, null).ConfigureAwait(false);
            if (refeshedValue.IsError == false)
            {
                context.Request.Headers.Add("Authorization", "Bearer " + refeshedValue.Data?.Token);
                await next(context);
            }
            else
            {
                await context.Response.WriteAsync(refeshedValue.ResponseException.Message, Encoding.UTF8).ConfigureAwait(false); ;//localize
            }
        }
    }
}


//public class JwtValidationHttpMessageHandler : DelegatingHandler
//{
//    private static SemaphoreSlim semaphore = new SemaphoreSlim(1);
//    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//    {

//        var response = await base.SendAsync(request, cancellationToken);
//        if (response.StatusCode == HttpStatusCode.Unauthorized &&
//  request.Headers.Where(c => c.Key == "Authorization")
//          .Select(c => c.Value)
//          .Any(c => c.Any(p => p.StartsWith("Bearer"))))
//        {
//            await semaphore.WaitAsync();
//        }
//        var pairs = new List<KeyValuePair<string, string>>
//            {
//                new KeyValuePair<string, string>("grant_type", "refresh_token"),
//                new KeyValuePair<string, string>("refresh_token", yourRefreshToken),
//                new KeyValuePair<string, string>("client_id", yourApplicationId),
//            };

//        //retry do to token request
//        using (var refreshResponse = await base.SendAsync(
//            new HttpRequestMessage(HttpMethod.Post,
//               new Uri(new Uri(Host), "Token"))
//            {
//                Content = new FormUrlEncodedContent(pairs)
//            }, cancellationToken))
//        {
//            var rawResponse = await refreshResponse.Content.ReadAsStringAsync();
//            var x = JsonConvert.DeserializeObject<RefreshToken>(rawResponse);

//            //new tokens here!
//            //x.access_token;
//            //x.refresh_token;

//            //to be sure
//            request.Headers.Remove("Authorization");
//            request.Headers.Add("Authorization", "Bearer " + x.access_token);

//            //headers are set, so release:
//            semaphore.Release();

//            //retry actual request with new tokens
//            response = await base.SendAsync(request, cancellationToken);

//        }
//    }

//        return response;
//    }
//}
