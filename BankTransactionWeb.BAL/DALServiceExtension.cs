using BankTransactionWeb.DAL.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace BankTransactionWeb.BAL
{
    public static class DALServiceExtension
    {
        public static void AddDALServices(this IServiceCollection services)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            services.AddDbContext<BankTransactionContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //services.AddTransient<YourDomainRepository>();
        }
    }
}
