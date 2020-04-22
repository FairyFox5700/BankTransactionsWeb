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
                c.AddProfile(new MapperProfile());
                c.AddProfile(new BisnessToDomainProfile());
            });

            return services.AddSingleton(mapperConfiguration.CreateMapper());
        }
    }
}
