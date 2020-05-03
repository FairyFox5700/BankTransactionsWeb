
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.Infrastucture;
using BankTransaction.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BankTransaction.BAL.Implementation.Extensions
{
    public static class CacheResolverExtension
    {
        public static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisOptions = new RedisCacheConfiguration();
            configuration.GetSection("RedisCache").Bind(redisOptions);
            services.AddSingleton(redisOptions);

            if (!redisOptions.Enabled)
            {
                return services;
            }
            services.AddSingleton<IConnectionMultiplexer>(mx => ConnectionMultiplexer.Connect(redisOptions.ConnectionString));
            services.AddStackExchangeRedisCache(options => options.Configuration = redisOptions.ConnectionString);
            services.AddSingleton<IRedisCacheService, RedisCacheService>();

            return services;
        }
    }
}
