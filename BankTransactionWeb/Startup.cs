using BankTransaction.BAL.Implementation.Extensions;
using BankTransaction.DAL.Implementation.Core;
using BankTransaction.DAL.Implementation.Extensions;
using BankTransaction.Entities;
using BankTransaction.Web.Extensions;
using BankTransaction.Web.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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

            #region Localization
            //https://riptutorial.com/asp-net-core/example/9728/localization-using-json-language-resources
            // services.AddSingleton<IdentityLocalizationService>();//??
            //https://github.com/dradovic/Embedded.Json.Localization/blob/master/src/Embedded.Json.Localization/JsonStringLocalizer.cs

            // var options = new LocalizationOptions { ResourcesPath = "Localization/Languages" };
            //IOptions<LocalizationOptions> localizationOptions = new LocalizationOptions() { ResourcesPath = "Localization/Languages" };
            services.TryAddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("fr-FR"),
            };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture: "en_US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
            });
           
            //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = @"Localization\Languages";
            });
            
            //services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
            //    opts => { opts.ResourcesPath = "Localization/Languages"; })
            //    .AddDataAnnotationsLocalization(opts => { });
            services.AddMvc().AddViewLocalization();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //_e = localizerFactory.Create(null);
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
                app.UseHsts();
                app.UseExceptionHandler("/Home/Error");
            }
            #region Localization

            //var localizationOpts = app.ApplicationServices.GetService<RequestLocalizationOptions>();
            //localizationOpts.RequestCultureProviders.Insert(0, new JsonRequestCultureProvider(configuration));


            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            #endregion

            app.UseStaticFiles();
            app.UseStatusCodePages();

            app.UseCors(builder => builder.WithOrigins("https://en.wikipedia.org", "http://localhost:64943")
                            .AllowAnyHeader()
                            .AllowAnyMethod().AllowCredentials());
            app.UseRouting();
            app.UseRequestLocalization();
            app.UseAuthentication();
            app.UseAuthorization();
            //https://github.com/DmitrySikorsky/AspNetCoreCultureRouteParameter

            //MyIdentityDataInitializer.SeedData(userManager, roleManager, context);//, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, BankTransactionContext context
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "Area",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "culture-route", 
                    pattern: "{culture=en-US}/{controller=Home}/{action=Index}/{id?}");
               
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }

    //public class RouteCultureCodesRequestCultureProvider : RequestCultureProvider
    //{
    //    public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    //    {
    //        string cultureCode = null;
    //        if (httpContext.Request.Path.HasValue && httpContext.Request.Path.Value == "/")
    //            cultureCode = this.GetDefaultCultureCode();
    //        else if (httpContext.Request.Path.HasValue && httpContext.Request.Path.Value.Length >= 4)

    //        {
    //            var parts = httpContext.Request.Path.Value.Split('/');
    //            if (!this.IsCulrureCodeValid(parts[0]))
    //                cultureCode = this.GetDefaultCultureCode(); //throw new HttpException(HttpStatusCode.NotFound);
    //        }
    //        else cultureCode = this.GetDefaultCultureCode();
    //        ProviderCultureResult requestCulture = new ProviderCultureResult(cultureCode);

    //        return Task.FromResult(requestCulture);
    //    }


    //    private string GetDefaultCultureCode()
    //    {
    //        return this.Options.DefaultRequestCulture.Culture.TwoLetterISOLanguageName;
    //    }

    //    private bool IsCulrureCodeValid(string cultureCode)
    //    {
    //        return this.Options.SupportedCultures.Select(c => c.TwoLetterISOLanguageName).Contains(cultureCode);
    //    }
    //}
}
