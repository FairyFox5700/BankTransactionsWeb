using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation;
using BankTransaction.BAL.Implementation.Extensions;
using BankTransaction.BAL.Implementation.Infrastucture;
using BankTransaction.Configuration;
using BankTransaction.Configuration.Extension;
using BankTransaction.DAL.Abstract;
using BankTransaction.DAL.Implementation.Core;
using BankTransaction.DAL.Implementation.EfCoreDAL;
using BankTransaction.DAL.Implementation.Extensions;
using BankTransaction.DAL.Implementation.Repositories.EFRepositories;
using BankTransaction.Entities;
using BankTransaction.Models.Mapper;
using BankTransaction.Models.Validation;
using BankTransaction.Web.Configuration;
using BankTransaction.Web.Helpers;
using BankTransaction.Web.Mapper;
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
            services.AddCors();
          
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddMapperViewConfiguration();
            services.AddDALServices(Configuration);
            services.AddBalServices(Configuration);
            services.AddJwtAuthentication(Configuration);
            services.AddDistributedCache(Configuration);
            services.AddIdentiyConfig();
            services.AddMvc();

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

            //app.UseCors(c=>c.SetIsOriginAllowed(x=>_=true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseStaticFiles();
            app.UseStatusCodePages();

            app.UseCors(builder => builder.WithOrigins("https://en.wikipedia.org", "http://localhost:64943")
                            .AllowAnyHeader()
                            .AllowAnyMethod().AllowCredentials()); 
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
