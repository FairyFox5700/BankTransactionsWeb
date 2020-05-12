using BankTransaction.DAL.Abstract;
using BankTransaction.DAL.Implementation.Core;
using BankTransaction.DAL.Implementation.Repositories.EFRepositories;
using BankTransaction.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.DAL.Implementation.Extensions
{
    public static class AddDALServicesExtension
    {

        public static IServiceCollection AddDALServices(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConfig = 
            services.AddDbContext<BankTransactionContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IShareholderRepository, ShareholderRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<BankTransactionContext>()
          .AddDefaultTokenProviders().AddUserManager<UserManager<ApplicationUser>>();
            return services;
        }

    }
}
//services.AddSingleton<InMemoryContainer>();
//services.AddTransient<IPersonRepository, PersonInMemoryRepository>();
//services.AddTransient<IAccountRepository, AccountInMemoryRepository>();
//services.AddTransient<ICompanyRepository, CompanyInMemoryRepository>();
//services.AddTransient<IShareholderRepository, ShareholderInMemoryRepository>();
//services.AddTransient<ITransactionRepository, TransactionInMemoryRepository>();
//services.AddScoped<IUnitOfWork, UnitOfWorkInMemory>();