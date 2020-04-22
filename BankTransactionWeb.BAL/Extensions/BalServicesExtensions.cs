
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.Infrastucture;
using BankTransaction.BAL.Implementation.RestApi;
using BankTransaction.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.BAL.Implementation.Extensions
{
    public static class BalServicesExtensions
    {
        public static IServiceCollection AddBALServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IShareholderService, ShareholderService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IAuthenticationService, BAL.Implementation.Infrastucture.AuthenticationService>();
            services.AddTransient<ISender, EmailSender>();
            services.AddTransient<IAdminService, AdminService>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.AddTransient<IJwtSecurityService, JWTSecurityService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfig>();
            services.AddSingleton(emailConfig);
            return services;
        }
    }
}
