using BankTransaction.BAL.Implementation.RestApi;
using BankTransaction.Models.Validation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Implementation.Extensions
{
   public static class JWTAuthenticationResolver
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind("Jwt", jwtSettings);
            services.AddSingleton(jwtSettings);
            var tokenValidParam = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ValidateIssuerSigningKey = true
            };
            services.AddSingleton(tokenValidParam);
            services.AddTransient<JWTSecurityService>();
            services.AddScoped<RevokeCookieAuthentication>();
            services.AddScoped<RedirectToLoginCookieAuthentication>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = tokenValidParam;
                 options.RequireHttpsMetadata = false;
                 options.SaveToken = true;
                 options.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         var accessToken = context.Request.Query["access_token"];

                         var path = context.HttpContext.Request.Path;
                         if (!string.IsNullOrEmpty(accessToken) &&
                             (path.StartsWithSegments("/agentsChat")))
                         {
                             context.Token = accessToken;
                         }
                         return Task.CompletedTask;
                     },

                     OnAuthenticationFailed = context =>
                     {
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };

             });
            return services;
        }
    }
}
//.AddCookie(options =>
//{

//    options.EventsType = typeof(RevokeCookieAuthentication);
//    options.LoginPath = "/ApiAuthentication/SignIn";
//    options.LogoutPath = "/ApiAuthentication/Signout";
//    //options.Cookie.Name = "BankWeb.AspNetCore.ProductKey";
//    //options.Cookie.ExpireTimeSpan = TimeSpan.FromMinutes(3);//only for test
//    options.Cookie.HttpOnly = true;
//    options.Cookie.SecurePolicy = environment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
//    options.Cookie.SameSite = SameSiteMode.None;
//})

//services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//   .AddJwtBearer(options => {
//       options.TokenValidationParameters = new TokenValidationParameters
//       {
//           ValidateIssuerSigningKey = true,
//           IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
//               .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
//           ValidateIssuer = false,
//           ValidateAudience = false
//       };
//   });

//services.Configure<CookiePolicyOptions>(options =>
//{
//    options.MinimumSameSitePolicy = SameSiteMode.None;
//    options.HttpOnly = HttpOnlyPolicy.Always;
//    options.Secure = CookieSecurePolicy.Always;

//    //options.CheckConsentNeeded = context => true;
//    ////options.MinimumSameSitePolicy = SameSiteMode.None;
//    ////options.HttpOnly = HttpOnlyPolicy.Always;
//    ////options.Secure = CookieSecurePolicy.Always;
//    //options.MinimumSameSitePolicy = SameSiteMode.Strict;
//    //options.HttpOnly = HttpOnlyPolicy.None;
//    //options.Secure = environment.IsDevelopment()
//    //  ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
//});