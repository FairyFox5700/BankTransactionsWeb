
using BankTransaction.Models.Validation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BankTransaction.Web.Helpers
{
    public class AccessTokenActionFilterAttribute : ActionFilterAttribute
    {
        private readonly IConfiguration configuration;

        public AccessTokenActionFilterAttribute(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind("Jwt", jwtSettings);
            var principal = context.HttpContext.User as ClaimsPrincipal;
            var accessTokenClaim = principal?.Claims
             .FirstOrDefault(c => c.Type == "access_token");
            if (accessTokenClaim is null || string.IsNullOrEmpty(accessTokenClaim.Value))
            {
                context.HttpContext.Response.Redirect("/ApiAuthentication/SignIn", permanent: true);

                return;
            }
            var validationParameters = new TokenValidationParameters
            {

                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            };
            var accessToken = accessTokenClaim.Value;
            var handler = new JwtSecurityTokenHandler();
            var user = (ClaimsPrincipal)null;
            try
            {
                user = handler.ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);
            }
            catch (SecurityTokenValidationException exception)
            {
                throw new Exception($"Token failed validation: {exception.Message}");
            }

            base.OnActionExecuting(context);
        }
    }
}
