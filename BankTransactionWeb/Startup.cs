using BankTransaction.BAL.Implementation.Extensions;
using BankTransaction.DAL.Implementation.Core;
using BankTransaction.DAL.Implementation.Extensions;
using BankTransaction.Entities;
using BankTransaction.Web.Extensions;
using BankTransaction.Web.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BankTransaction.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddMapperViewConfiguration();
            services.AddDALServices(Configuration);
            services.AddBalServices(Configuration);
            services.AddJwtAuthentication(Configuration,Environment);
            services.AddDistributedCache(Configuration);
            services.AddIdentiyConfig();
            services.AddJsonLocalization();
            services.AddViewServices();
            services.AddMvc(options => options.Filters.Add(typeof(ExceptionHandlerFilter)));
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
                app.UseExceptionHandler("Home/Error");
                app.UseSpaStaticFiles();
                app.UseHsts();
            }

            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);
           
            app.UseStaticFiles();
            app.UseStatusCodePages();
            //app.UseStatusCodePagesWithReExecute("/Error/{statusCode}");

            app.UseCors(builder => builder.WithOrigins("https://en.wikipedia.org", "http://localhost:64943")
                            .AllowAnyHeader()
                            .AllowAnyMethod().AllowCredentials());
            app.UseRouting();

            //app.UseCookiePolicy(new CookiePolicyOptions
            //{
            //    MinimumSameSitePolicy = SameSiteMode.Strict,
            //    HttpOnly = HttpOnlyPolicy.None,//only for devepoment
            //    Secure = CookieSecurePolicy.Always
            //});
            app.UseSecurityJwt();
            app.UseAuthentication();
            app.UseAuthorization();
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
