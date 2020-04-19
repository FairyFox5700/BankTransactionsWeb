using AutoMapper;
using BankTransaction.Api.Controllers;
using BankTransaction.Api.Helpers;
using BankTransaction.Api.Models;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation;
using BankTransaction.BAL.Implementation.Infrastucture;
using BankTransaction.Configuration;
using BankTransaction.DAL.Abstract;
using BankTransaction.DAL.Implementation.Core;
using BankTransaction.DAL.Implementation.EfCoreDAL;
using BankTransaction.DAL.Implementation.Repositories.EFRepositories;
using BankTransaction.Entities;
using BankTransaction.Models.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BankTransaction.Api
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            IMapper mapper = new AutoMapper.Mapper(AutoMapperConfiguration.ConfigureAutoMapper());
            services.AddSingleton(mapper);
            //services.AddDALServices();
            services.AddDbContext<BankTransactionContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
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
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IAuthenticationService, BAL.Implementation.Infrastucture.AuthenticationService>();


            services.AddTransient<ISender, EmailSender>();
            services.AddTransient<IAdminService, AdminService>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuthenticationService, BAL.Implementation.Infrastucture.AuthenticationService>();
            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfig>();
            services.AddSingleton(emailConfig);
            services.AddTransient<IJwtSecurityService, JWTSecurityService>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<BankTransactionContext>()
            .AddDefaultTokenProviders().AddUserManager<UserManager<ApplicationUser>>();

            var jwtSettings = new JwtSettings();
            Configuration.Bind("Jwt", jwtSettings);
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.TokenValidationParameters = tokenValidParam;
             });
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // app.ConfigureErrorHandler();
            app.UseAPIResponseWrapperMiddleware();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
