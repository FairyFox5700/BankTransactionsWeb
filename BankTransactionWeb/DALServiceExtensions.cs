
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation;
using BankTransaction.DAL.Abstract;
using BankTransaction.DAL.Implementation.Core;
using BankTransaction.DAL.Implementation.EfCoreDAL;
using BankTransaction.DAL.Implementation.Repositories.EFRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using IAuthenticationService = BankTransaction.BAL.Abstract.IAuthenticationService;

namespace BankTransaction.Web
{
    public static class DALServiceExtension
    {
        //public static void AddDALServices(this IServiceCollection services)
        //{

        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //    .AddJsonFile("appsettings.json")
        //    .Build();

        //    services.AddDbContext<BankTransactionContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        //    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        //    services.AddTransient<IPersonRepository, PersonRepository>();
        //    services.AddTransient<IAccountRepository, AccountRepository>();
        //    services.AddTransient<ICompanyRepository, CompanyRepository>();
        //    services.AddTransient<IShareholderRepository, ShareholderRepository>();
        //    services.AddTransient<ITransactionRepository, TransactionRepository>();
        //    services.AddScoped<IUnitOfWork, UnitOfWork>();
        //    //services.AddSingleton<InMemoryContainer>();
        //    //services.AddTransient<IPersonRepository, PersonInMemoryRepository>();
        //    //services.AddTransient<IAccountRepository, AccountInMemoryRepository>();
        //    //services.AddTransient<ICompanyRepository, CompanyInMemoryRepository>();
        //    //services.AddTransient<IShareholderRepository, ShareholderInMemoryRepository>();
        //    //services.AddTransient<ITransactionRepository, TransactionInMemoryRepository>();
        //    //services.AddScoped<IUnitOfWork, UnitOfWorkInMemory>();
        //    services.AddTransient<ICompanyService, CompanyService>();
        //    services.AddTransient<ITransactionService, TransactionService>();
        //    services.AddTransient<IPersonService, PersonService>();
        //    services.AddTransient<IAccountService, AccountService>();
        //    services.AddTransient<IShareholderService, ShareholderService>();
        //    services.AddTransient<IAuthenticationService, BAL.Implementation.Infrastucture.AuthenticationService>();
          
           
        //    services.AddTransient<ISender, EmailSender>();
        //    services.AddTransient<IAdminService, AdminService>();
        //    services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        //    services.TryAddSingleton<IUrlHelperFactory, UrlHelperFactory>();
        //    services.TryAddSingleton<IHttpContextAccessor,HttpContextAccessor>();
        //    services.AddTransient<IAuthenticationService, BAL.Implementation.Infrastucture.AuthenticationService>();
        //    var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfig>();
        //    services.AddSingleton(emailConfig);


        //}
    }
}
