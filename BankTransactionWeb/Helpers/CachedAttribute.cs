﻿using BankTransaction.BAL.Abstract;
using BankTransaction.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.Web.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : ResultFilterAttribute, IAsyncActionFilter
    {
        private readonly int timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            this.timeToLiveInSeconds = timeToLiveInSeconds;
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            //nothing for you there
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ContentResult)
            {
                context.Cancel = true;
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

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IRedisCacheService>();
            var cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var httpResponse = context.HttpContext.Response;
            var responseStream = httpResponse.Body;
            responseStream.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(responseStream, Encoding.UTF8, true, 512, true))
            {
                var toCache = streamReader.ReadToEnd();
                var contentType = httpResponse.ContentType;
                var statusCode = httpResponse.StatusCode.ToString();
                Task.Factory.StartNew(() =>
                {
                    cacheService.CacheHtmlResponce(cacheKey + "_contentType", contentType, TimeSpan.FromSeconds(timeToLiveInSeconds));
                    cacheService.CacheHtmlResponce(cacheKey + "_statusCode", statusCode, TimeSpan.FromSeconds(timeToLiveInSeconds));
                    cacheService.CacheHtmlResponce(cacheKey, toCache, TimeSpan.FromSeconds(timeToLiveInSeconds));
                });

            }
            base.OnResultExecuted(context);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IRedisCacheService>();
            var cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var cachedData = await cacheService.Get<string>((cacheKey));
            var contentType = await cacheService.Get<string>(cacheKey + "_contentType");
            var statusCode = await cacheService.Get<string>(cacheKey + "_statusCode");
            if (!string.IsNullOrEmpty(cachedData) && !string.IsNullOrEmpty(contentType) &&
                !string.IsNullOrEmpty(statusCode))
            {
                //cache hit
                var httpResponse = context.HttpContext.Response;
                httpResponse.ContentType = contentType;
                httpResponse.StatusCode = Convert.ToInt32(statusCode);

                var responseStream = httpResponse.Body;
                responseStream.Seek(0, SeekOrigin.Begin);
                if (responseStream.Length <= cachedData.Length)
                {
                    responseStream.SetLength((long)cachedData.Length << 1);
                }
                using (var writer = new StreamWriter(responseStream, Encoding.UTF8, 4096, true))
                {
                    writer.Write(cachedData);
                    writer.Flush();
                    responseStream.Flush();
                    context.Result = new ContentResult { Content = cachedData };
                }
                
            }
            else
            {
                //cache miss
            }
        }
    }
}



