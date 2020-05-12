using BankTransaction.Models.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Implementation.RestApi
{

    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly IDistributedCache cache;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly JwtSettings jwtSettings; 

        public JwtTokenManager(IDistributedCache cache,
                IHttpContextAccessor httpContextAccessor,
               JwtSettings jwtSettings)
        {
            this.cache = cache;
            this.httpContextAccessor = httpContextAccessor;
            this.jwtSettings = jwtSettings;
        }
        public async Task DeactivateCurrentTokenAsync()
          => await DeactivateTokenAsync(GetCurrentAsync());


        private string GetCurrentAsync()
        {
            var authorizationHeader = httpContextAccessor
               .HttpContext.Request.Headers[HeaderNames.Authorization];
            var authorizationHeader2 = httpContextAccessor
               .HttpContext.Request.Headers["Authorization"];//remove
            return String.IsNullOrEmpty(authorizationHeader)
                ? string.Empty
                : authorizationHeader.ToString();
        }

        public async Task DeactivateTokenAsync(string token)
        => await  cache.SetStringAsync(GetKey(token),
                   " ", new DistributedCacheEntryOptions
                   {
                       AbsoluteExpirationRelativeToNow =
                           jwtSettings.TokenLifeTime
                   });

        public async  Task<bool> IsCurrentTokenActive()
        {
           return  await IsTokenActiveAsync(GetCurrentAsync());
        }

        public async Task<bool> IsTokenActiveAsync(string token)
        {
            return await cache.GetStringAsync(GetKey(token)) == null;
        }

        private static string GetKey(string token)
            => $"DeactivatedTokens:{token}";
    }
}
