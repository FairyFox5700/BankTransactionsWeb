using BankTransaction.Api.Helpers;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Api.Extensions
{
    public static class JwtTokenManagerMiddlewareResolver
    {
        public static IApplicationBuilder AddJwtTokenManager(this IApplicationBuilder builder) => builder.UseMiddleware<JwtTokenMangerMiddleware>();
    }
    
}
