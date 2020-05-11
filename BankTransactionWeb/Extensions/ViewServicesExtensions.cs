using BankTransaction.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Extensions
{
    public static class ViewServicesExtensions
    {
        public static IServiceCollection AddViewServices(this IServiceCollection services)
        {
            services.AddTransient<IApiBankAccountService, ApiAccountService>();
            services.AddTransient<IApiLoginService, ApiLoginService>();
            return services;
        }
    }
}
