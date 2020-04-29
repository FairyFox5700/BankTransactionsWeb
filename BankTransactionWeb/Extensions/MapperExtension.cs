using AutoMapper;
using BankTransaction.Models.Mapper;
using BankTransaction.Web.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Configuration.Extension
{
    public static class MapperExtension
    {
        public static IServiceCollection AddMapperViewConfiguration(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(c =>
            {
               // c.AddProfile(new MapperProfile());
                c.AddProfile(new BisnessToDomainProfile());
                c.AddProfile(new FilterMapperProfile());
                c.AddProfile(new PersonMapperProfile());
                c.AddProfile(new AccountMapperProfile());
                c.AddProfile(new CompanyMapperProfile());
                c.AddProfile(new ShareholderMapperProfile());
                c.AddProfile(new TransactioMapperProfile());
                c.AddProfile(new IdentityMapperProfile());
                c.AddProfile(new RoleMapperProfile());
            });

            return services.AddSingleton(mapperConfiguration.CreateMapper());
        }
    }
}
