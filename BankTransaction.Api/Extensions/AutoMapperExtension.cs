using AutoMapper;
using BankTransaction.Api.Mapper;
using BankTransaction.Models.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace BankTransaction.Api.Extensions
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddMapperDomainConfiguration(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(c =>
            {
                c.AddProfile(new ApiModelToBissnessMap());
                c.AddProfile(new BisnessToDomainProfile());
            });

            return services.AddSingleton(mapperConfiguration.CreateMapper());
        }
    }
}


