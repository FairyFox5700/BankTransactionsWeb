using Microsoft.AspNetCore.Builder;

namespace BankTransaction.Api.Helpers
{
    //    public class ResponceResult<T> : IActionResult
    //    {
    //        private readonly HttpStatusCode statusCode;
    //        private readonly T data;
    //        private readonly HttpRequestMessage httpRequest;

    //        public ResponceResult(HttpStatusCode statusCode, T data, HttpRequestMessage httpRequest)
    //        {
    //            this.statusCode = statusCode;
    //            this.data = data;
    //            this.httpRequest = httpRequest;
    //        }
    //        public async Task ExecuteResultAsync(ActionContext context)
    //        {
    //            var apiResponce = new ApiResponse((int)statusCode, data);

    //            try
    //            {
    //                var result = new JsonResult(apiResponce);
    //                if (!context.ModelState.IsValid)
    //                {
    //                    var errorListModel = context.ModelState
    //                        .Where(x => x.Value.Errors.Count > 0)
    //                        .ToDictionary(er => er.Key, er => er.Value.Errors.Select(x => x.ErrorMessage))
    //                        .ToList();
    //                    var errorResponce = new ApiErrorResonse();
    //                    foreach (var error in errorListModel)
    //                    {
    //                        foreach (var erValue in error.Value)
    //                        {
    //                            var errorModel = new ValidationError()
    //                            {
    //                                Name = error.Key,
    //                                Message = erValue
    //                            };
    //                            errorResponce.ValidationErrors.Add(errorModel);
    //                        }
    //                    }

    //                }
    //                await result.ExecuteResultAsync(context);
    //            }
    //            catch (Exception ex)
    //            {
    //                var result = new JsonResult(apiResponce);
    //                if (apiResponce.ResponseException == null)
    //                {
    //                    apiResponce.ResponseException = new ApiErrorResonse() { Message = ex.Message };
    //                }
    //                else
    //                {
    //                    apiResponce.ResponseException.Message = ex.Message;
    //                }
    //                await result.ExecuteResultAsync(context);
    //            }

    //        }

    //    }
    //}


    public static class ApiResponseMiddlewareExtension
    {
        public static IApplicationBuilder UseAPIResponseWrapperMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiResponseMiddleware>();
        }
    }
}

