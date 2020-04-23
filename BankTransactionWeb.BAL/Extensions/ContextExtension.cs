using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace BankTransaction.BAL.Implementation.Extensions
{
    public static class ContextExtension
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return String.Empty;
            }
            return httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}
