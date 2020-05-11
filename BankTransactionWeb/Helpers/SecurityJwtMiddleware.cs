using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BankTransaction.Web.Helpers
{
    public class SecurityJwtMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IApiLoginService apiLoginService;

        public SecurityJwtMiddleware(RequestDelegate next, IApiLoginService apiLoginService)
        {
            this.next = next;
            this.apiLoginService = apiLoginService;
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

    }

    //private ActionResult ValidateApiResult<T>(ApiDataResponse<T> result)//ValidateCustomnLogic
    //{
    //    if (result.IsError)
    //    {
    //        return RedirectToAction("Error", "Home", result.ResponseException.Message);
    //    }
    //    return null;
    //}



    //public class JwtValidationHandler
    //{
    //    private readonly IApiLoginService apiLoginService;
    //    private readonly ICookieHelperService helperService;
    //    private readonly ILogger<JwtValidationHandler> logger;

    //    public JwtValidationHandler( IApiLoginService apiLoginService, ICookieHelperService helperService, ILogger<JwtValidationHandler> logger)
    //    {
    //        this.apiLoginService = apiLoginService;
    //        this.helperService = helperService;
    //        this.logger = logger;
    //    }
    //    private Task OnAuthenticationFailed(AuthenticationFailedContext context)
    //    {
    //        logger.LogError($"Authentication Failed(OnAuthenticationFailed): {context.Exception}");

    //        return Task.FromResult(0);
    //    }
    //}



    //public class JwtValidationHttpMessageHandler : DelegatingHandler
    //{
    //    private readonly IServiceProvider _serviceProvider;

    //    public JwtValidationHttpMessageHandler(IServiceProvider serviceProvider)
    //    {
    //        _serviceProvider = serviceProvider;
    //    }

    //    protected JwtValidationHttpMessageHandler(HttpMessageHandler innerHandler, IServiceProvider serviceProvider) : base(innerHandler)
    //    {
    //    }
    //    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        return await base.SendAsync(request, cancellationToken);
    //    }


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
    //        var token = context.Request.Cookies["BankWeb.AspNetCore.ProductKey"];
    //        var refreshtoken = context.Request.Cookies["BankWeb.AspNetCore.ProductKeyFree"];
    //        var pairs = new List<KeyValuePair<string, string>>
    //        {
    //            new KeyValuePair<string, string>("grant_type", "refresh_token"),
    //            new KeyValuePair<string, string>("refresh_token", yourRefreshToken),
    //            new KeyValuePair<string, string>("client_id", yourApplicationId),
    //        };

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

    //    return response;
    //}
}
