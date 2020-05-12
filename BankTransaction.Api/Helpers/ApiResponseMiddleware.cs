using BankTransaction.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
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
                    //Return result from inner midlleware
                    await next(context);
                    var feature = context.Features.Get<ModelStateFeature>();
                    var ModelState = context.Features.Get<ModelStateFeature>()?.ModelState;
                    var body = await FormatResponse(context.Response);

                    if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        //var body = await FormatResponse(context.Response);
                        await HandleSuccessRequestAsync(context, body, context.Response.StatusCode);

                    }
                    else if (ModelState != null && !ModelState.IsValid)
                    {
                        var error = HandleNotSuccessValidationAsync(context, context.Response.StatusCode, ModelState);
                    }
                    else
                    {

                        await HandleNotSuccessRequestAsync(context, context.Response.StatusCode, body);
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

        private Task HandleNotSuccessRequestAsync(HttpContext context, int statusCode, string body)
        {
            context.Response.ContentType = "application/json";
            var apiErrorResponce = JsonConvert.DeserializeObject<ApiErrorResponse>(body);
            if (apiErrorResponce != null && apiErrorResponce is ApiErrorResponse)
                apiErrorResponce = apiErrorResponce;
            else
                apiErrorResponce = new ApiErrorResponse { Message = "Your request cannot be processed. Please contact a support." };


            var apiResponce = new ApiResponse(statusCode, apiErrorResponce);
            context.Response.StatusCode = statusCode;
            var json = JsonConvert.SerializeObject(apiResponce);
            return context.Response.WriteAsync(json);
        }

        private Task HandleNotSuccessValidationAsync(HttpContext context, int statusCode, ModelStateDictionary modelState)
        {
            context.Response.ContentType = "application/json";
            var errorListModel = modelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(er => er.Key, er => er.Value.Errors.Select(x => x.ErrorMessage))
                .ToList();
            var apiErrorResponce = new ApiErrorResponse();
            foreach (var error in errorListModel)
            {
                foreach (var erValue in error.Value)
                {
                    var errorModel = new ValidationError()
                    {
                        Name = error.Key,
                        Message = erValue
                    };
                    apiErrorResponce.ValidationErrors.Add(errorModel);
                }
            }
            var apiResponce = new ApiResponse(statusCode, apiErrorResponce);
            context.Response.StatusCode = statusCode;
            var json = JsonConvert.SerializeObject(apiResponce);
            return context.Response.WriteAsync(json);
        }


        private Task HandleSuccessRequestAsync(HttpContext context, object body, int statusCode)
        {
            context.Response.ContentType = "application/json";
            string jsonString, bodyText = string.Empty;

            if (!IsValidJson(body.ToString(), logger))
                bodyText = JsonConvert.SerializeObject(body);
            else
                bodyText = body.ToString();
            if (statusCode == StatusCodes.Status401Unauthorized)
            {
                jsonString = JsonConvert.SerializeObject(ApiResponse.Unauthorized);
                return context.Response.WriteAsync(jsonString);
            }
            else
            {
                dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
                var apiResponse = new ApiResponse(bodyContent, statusCode);
                jsonString = JsonConvert.SerializeObject(apiResponse);
            }
            return context.Response.WriteAsync(jsonString);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var apiErrorResponce = new ApiErrorResponse();
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
            logger.LogError($"An exception occured {exception.Message}");
            return context.Response.WriteAsync(json);

        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return plainBodyText;
        }

        private static bool IsValidJson(string input, ILogger<ApiResponseMiddleware> logger)
        {
            input = input.Trim();
            if ((input.StartsWith("{") && input.EndsWith("}")) || //For object
            (input.StartsWith("[") && input.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(input);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    logger.LogError(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    logger.LogError(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}

