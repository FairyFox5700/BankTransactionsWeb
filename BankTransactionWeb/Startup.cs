using AutoMapper;
using BankTransaction.Api.Controllers;
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
using BankTransaction.Web.Configuration;
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
using System;
using System.Text;

namespace BankTransaction.Web
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
            services.AddRazorPages().AddRazorRuntimeCompilation();
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
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 2;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, BankTransactionContext context)
        {
            if (env.IsDevelopment())
            {
                var options = new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 2
                };
                app.UseDeveloperExceptionPage(options);
            }

            else
            {

                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //MyIdentityDataInitializer.SeedData(userManager, roleManager, context);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "Area",
                 pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
