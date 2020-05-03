using BankTransaction.BAL.Abstract;
using BankTransaction.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.Api.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            this.timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheConfig = context.HttpContext.RequestServices.GetRequiredService<RedisCacheConfiguration>();
            if (!cacheConfig.Enabled)
            {
                await next();
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IRedisCacheService>();
            var cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var cachedData = await cacheService.GetCachedResponce(cacheKey);
            if (!String.IsNullOrEmpty(cachedData))
            {
                var contentResult = new ContentResult()
                {
                    Content = cachedData,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var executeNext = await next();
            //kpsdopfsdf
            var okObjectResult = executeNext.Result as OkObjectResult;
            if(executeNext.Result!=null )
            {
                await cacheService.CacheResponce(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(timeToLiveInSeconds));
            }
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var requestUrl = request.GetEncodedUrl();
            var hash = new StringBuilder();
            var md5provider = new MD5CryptoServiceProvider();
            var bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(requestUrl));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
