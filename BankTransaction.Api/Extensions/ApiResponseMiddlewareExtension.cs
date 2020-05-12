using BankTransaction.Api.Helpers;
using Microsoft.AspNetCore.Builder;

namespace BankTransaction.Api.Extensions
{
    public static class ApiResponseMiddlewareExtension
    {
        public static IApplicationBuilder UseAPIResponseWrapperMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiResponseMiddleware>();
        }
    }
}

