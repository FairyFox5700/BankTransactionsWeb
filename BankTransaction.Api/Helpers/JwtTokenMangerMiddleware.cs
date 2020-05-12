using BankTransaction.BAL.Implementation.RestApi;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BankTransaction.Api.Helpers
{
    public class JwtTokenMangerMiddleware
    {
        private readonly IJwtTokenManager jwtTokenManager;

        public JwtTokenMangerMiddleware(IJwtTokenManager jwtTokenManager)
        {
            this.jwtTokenManager = jwtTokenManager;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if( await jwtTokenManager.IsCurrentTokenActive())
            {
                await next(context);
                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

    }
}
