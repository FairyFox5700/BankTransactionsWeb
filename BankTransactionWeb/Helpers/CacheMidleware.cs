using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Helpers
{
    public class CachedMiddleware
    {
        protected RequestDelegate NextMiddleware;

        public CachedMiddleware(RequestDelegate nextMiddleware)
        {
            NextMiddleware = nextMiddleware;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var responseStream = new MemoryStream())
            {
                var fullResponse = httpContext.Response.Body;
                httpContext.Response.Body = responseStream;
                await NextMiddleware.Invoke(httpContext);
                responseStream.Seek(0, SeekOrigin.Begin);
                await responseStream.CopyToAsync(fullResponse);
            }
        }

    }
}
