using System;
using Identification.Areas.Identity.Data;
using Identification.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Identification.Areas.Identity.IdentityHostingStartup))]
namespace Identification.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<IdentificationContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("IdentificationContextConnection")));

                services.AddDefaultIdentity<IdentificationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<IdentificationContext>();
            });
        }
    }
}