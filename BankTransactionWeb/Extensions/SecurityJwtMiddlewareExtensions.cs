using BankTransaction.Web.Helpers;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Extensions
{
    public static class SecurityJwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityJwt(this IApplicationBuilder builder) => builder.UseMiddleware<SecurityJwtMiddleware>();
    }
}
