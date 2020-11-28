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
using Microsoft.AspNetCore.Builder;

namespace BankTransaction.Web.Helpers
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {

                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured: {ex.Message}. Details {ex.InnerException}");
                await HandleExceptionAsync(context, ex);
            }
        }
        private static bool IsRequestApi(HttpContext context)
        {
            return context.Request.Path.Value.ToLower().StartsWith("/api");
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if(exception.GetType() ==typeof(BankApiException))
            {
                context.Response.Redirect($"~/Error/{exception.Message}");
            }
            context.Response.StatusCode = 500;
            if (IsRequestApi(context))
            {
                //when request api 
                context.Response.Redirect($"Error/{exception.Message}");
                //context.Response.ContentType = "application/json";
                //await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                //{
                //    State = 500,
                //    message = exception.Message
                //}));
            }
            else
            {
                context.Response.Redirect("~Home/Error");
            }

        }


    }
   

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
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
    public class ExceptionHandlerFilter :Attribute, IActionFilter
    {

        public ExceptionHandlerFilter()
        {
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var objectResult = context.Result as ObjectResult;
            var apiResult = objectResult?.Value as ApiResponse;
            if (apiResult == null)
                return;
            if (apiResult.IsError == false)
                return;
            var message = apiResult.ResponseException.MessageType;
            var statusCode = apiResult.StatusCode;
            if (message == null)
                return;
            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new RedirectToRouteResult($"Error/{message}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            

        }
    }
}
