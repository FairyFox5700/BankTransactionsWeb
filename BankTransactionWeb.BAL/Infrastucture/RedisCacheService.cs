using BankTransaction.BAL.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task CacheResponce(string cacheKey, object responce, TimeSpan timeToLiveInSeconds)
        {

            if (responce == null)
            {
                return;
            }

            var serializedResponce = JsonConvert.SerializeObject(responce);

            await distributedCache.SetStringAsync(cacheKey, serializedResponce, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLiveInSeconds
            });
        }

        public async  Task<string> GetCachedResponce(string cacheKey)
        {
            var cachedResponce = await distributedCache.GetStringAsync(cacheKey);

            return String.IsNullOrEmpty(cachedResponce) ? null : cachedResponce;
        }
    }
}
