using AutoMapper;
using BankTransaction.Api.Extensions;
using BankTransaction.Api.Helpers;
using BankTransaction.Api.Models;
using BankTransaction.BAL.Implementation.Extensions;
using BankTransaction.DAL.Implementation.Extensions;
using BankTransaction.Models.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BankTransaction.Api
{

    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }


        private IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // IMapper mapper = new AutoMapper.Mapper(AutoMapperConfiguration.ConfigureAutoMapper());
            // services.AddSingleton(mapper);
            services.AddMapperDomainConfiguration();
            //services.AddAutoMapper(typeof(Startup));
            services.AddDALServices(Configuration);
            services.AddBalServices(Configuration);
            services.AddJwtAuthentication(Configuration, Environment);
            services.AddDistributedCache(Configuration);
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddMvc(opts =>
            {
            opts.Filters.Add(new ValidationFilter());
            });

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // app.ConfigureErrorHandler();
            //app.UseCookiePolicy(new CookiePolicyOptions
            //{
            //    MinimumSameSitePolicy = SameSiteMode.None
            //});
            app.UseAPIResponseWrapperMiddleware();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
