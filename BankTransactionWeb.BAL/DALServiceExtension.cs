using BankTransactionWeb.DAL.EfCoreDAL;
using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.EfCoreDAL.Repositories;
using BankTransactionWeb.DAL.Interfaces;
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
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IShareholderRepository, ShareholderRepository>();
            services.AddTransient<ITransactionRepository,TransactionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddTransient<YourDomainRepository>();
        }
    }
}
