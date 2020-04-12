using AutoMapper;
using BankTransactionWeb.BAL.Cofiguration;
using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RestWebBankTransactionApp
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
            IMapper mapper = new Mapper(AutoMapperConfiguration.ConfigureAutoMapper());
            services.AddSingleton(mapper);
            services.AddDALServices();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<BankTransactionContext>()
          .AddDefaultTokenProviders().AddUserManager<UserManager<ApplicationUser>>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.RequireHttpsMetadata = false;
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidIssuer = Configuration["Jwt:Issuer"],
                         ValidateAudience = true,
                         ValidAudience = Configuration["Jwt:Audience"],
                         ValidateLifetime = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyToGEnerateTokeJWT_DotC")),
                         ValidateIssuerSigningKey = true,
                     };
                 });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
