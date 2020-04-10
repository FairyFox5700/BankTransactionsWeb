using AutoMapper;
using BankTransactionWeb.BAL.Infrastucture;
using BankTransactionWeb.BAL.Infrastucture.MessageServices;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.DAL.EfCoreDAL;
using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.EfCoreDAL.Repositories;
using BankTransactionWeb.DAL.InMemoryDAL;
using BankTransactionWeb.DAL.InMemoryDAL.Repositories;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace BankTransactionWeb.BAL.Cofiguration
{
    public static class DALServiceExtension
    {
        public static void AddDALServices(this IServiceCollection services)
        {


            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IShareholderRepository, ShareholderRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddSingleton<InMemoryContainer>();
            //services.AddTransient<IPersonRepository, PersonInMemoryRepository>();
            //services.AddTransient<IAccountRepository, AccountInMemoryRepository>();
            //services.AddTransient<ICompanyRepository, CompanyInMemoryRepository>();
            //services.AddTransient<IShareholderRepository, ShareholderInMemoryRepository>();
            //services.AddTransient<ITransactionRepository, TransactionInMemoryRepository>();
            //services.AddScoped<IUnitOfWork, UnitOfWorkInMemory>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IShareholderService, ShareholderService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
          
           
            services.AddTransient<ISender, EmailSender>();
            services.AddTransient<IAdminService, AdminService>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.TryAddSingleton<IHttpContextAccessor,HttpContextAccessor>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            

        }
    }
}
