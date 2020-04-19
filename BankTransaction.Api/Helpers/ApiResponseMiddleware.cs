using BankTransaction.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BankTransaction.Api.Helpers
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ApiResponseMiddleware> logger;
        public ApiResponseMiddleware(RequestDelegate next, ILogger<ApiResponseMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {

            var mainBody = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {

                    await next(context);
                    if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        var body = await FormatResponse(context.Response);
                        await HandleSuccessRequestAsync(context, body, context.Response.StatusCode);

                    }
                    else
                    {
                        await HandleNotSuccessRequestAsync(context, context.Response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(context, ex);
                }
                finally
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(mainBody);
                }

            }
        }

        private Task HandleNotSuccessRequestAsync(HttpContext context, int statusCode)
        {
            context.Response.ContentType = "application/json";
            var apiErrorResponce = new ApiErrorResonse();
            //int code = (int)HttpStatusCode.InternalServerError;
            if (statusCode == (int)HttpStatusCode.NotFound)
                apiErrorResponce.Message = "The specified URI does not exist. Please verify and try again.";
            else if (statusCode == (int)HttpStatusCode.BadRequest)
                apiErrorResponce.Message = "The specified URI does not mach any pattern";
            else if (statusCode == (int)HttpStatusCode.NoContent)
                apiErrorResponce.Message = ("The specified URI does not contain any content.");
            var apiResponce = new ApiResponse(statusCode, apiErrorResponce);
            context.Response.StatusCode = statusCode;
            var json = JsonConvert.SerializeObject(apiResponce);
            return context.Response.WriteAsync(json);

        }


        private Task HandleSuccessRequestAsync(HttpContext context, object body, int statusCode)
        {
            context.Response.ContentType = "application/json";
            string jsonString, bodyText = string.Empty;
            bodyText = body.ToString();
            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);

            var apiResponse = new ApiResponse(statusCode, bodyContent);
            jsonString = JsonConvert.SerializeObject(apiResponse);

            return context.Response.WriteAsync(jsonString);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var apiErrorResponce = new ApiErrorResonse();
            int code = (int)HttpStatusCode.InternalServerError;
            if (exception is DbUpdateException)
            {

                apiErrorResponce.Message = exception.Message;
                code = (int)HttpStatusCode.BadRequest;
                context.Response.StatusCode = code;

            }
            //else if
            else
            {
                apiErrorResponce.Message = exception.Message;
                code = (int)HttpStatusCode.InternalServerError;
                context.Response.StatusCode = code;
            }

            context.Response.ContentType = "application/json";
            var apiResponce = new ApiResponse(code, apiErrorResponce);
            var json = JsonConvert.SerializeObject(apiResponce);

            return context.Response.WriteAsync(json);

        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return plainBodyText;
        }
    }
}

