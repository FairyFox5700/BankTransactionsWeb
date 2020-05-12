using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Implementation.Extensions
{
    public class RevokeCookieAuthentication : CookieAuthenticationEvents
    {
        private readonly IDistributedCache cache;
        public RevokeCookieAuthentication(IDistributedCache cache)
        {
            this.cache = cache;
        }
        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userId = context.Principal?.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!string.IsNullOrEmpty(cache.GetString("revoke-" + userId)))
            {
                context.RejectPrincipal();

                cache.Remove("revoke-" + userId);
            }
            return Task.CompletedTask;
        }
    }
}
