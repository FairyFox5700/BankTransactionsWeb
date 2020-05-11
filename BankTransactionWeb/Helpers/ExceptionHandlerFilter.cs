using BankTransaction.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Api.Models.Responces;
using BankTransaction.Web.Localization;

namespace BankTransaction.Web.Helpers
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {

                await next(context);
                //if (context.Response.StatusCode == 401)  //page not found
                //{
                //    await HandleFor404Async(context);
                //}
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static bool IsRequestApi(HttpContext context)
        {
            return context.Request.Path.Value.ToLower().StartsWith("/api");
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = 500;
            if (IsRequestApi(context))
            {
                //when request api 
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    State = 500,
                    message = exception.Message
                }));
            }
            else
            {
                //when request page 
                context.Response.Redirect("/Home/Errorpage");
            }

        }


    }

    //public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    //{
    //    public override void OnException(ExceptionContext context)
    //    {
    //        if (context.Exception is ValidationException validationException)
    //        {
    //            var response = new ApiErrorResponse
    //            {
    //                Errors = validationException.Errors.Select(x => new ApiErrorResponse.Error(x))
    //            };
    //            context.Result = new BadRequestObjectResult(response);
    //            context.ExceptionHandled = true;
    //        }
    //    }
    //}
    public class ExceptionHandlerFilter : IActionFilter
    {
        private readonly ILogger<ExceptionHandlerFilter> logger;
        private readonly IStringLocalizer<ApiResponcesShared> sharedLocalizer;

        public ExceptionHandlerFilter(ILogger<ExceptionHandlerFilter> logger, IStringLocalizer<ApiResponcesShared> sharedLocalizer)
        {
            this.logger = logger;
            this.sharedLocalizer = sharedLocalizer;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var objectResult = context.Result as ObjectResult;
            var apiResult = objectResult?.Value as ApiResponse;
            if (apiResult == null)
                return;
            if (apiResult.IsError == false)
                return;
            var message = sharedLocalizer[apiResult.ResponseException.MessageType] ?? apiResult.ResponseException.Message;
            var statusCode = apiResult.StatusCode;
            if (message == null)
                return;
            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new RedirectToRouteResult($"Error/{message}");
            logger.LogError(0, apiResult.ResponseException.MessageType, "An exception has occurred: " + apiResult.ResponseException.MessageType);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            

        }
    }
}
