using BankTransaction.BAL.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Text;
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

        public async Task CacheHtmlResponce(string cacheKey, object responce, TimeSpan timeToLiveInSeconds)
        {
            string toStore;
            if (responce is string)
            {
                toStore = (string)responce;
            }
            else
            {
                toStore = JsonConvert.SerializeObject(responce);
            }


            await distributedCache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(toStore), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLiveInSeconds
            });
        }

        public async  Task<string> GetCachedResponce(string cacheKey)
        {
            var cachedResponce = await distributedCache.GetStringAsync(cacheKey);

            return String.IsNullOrEmpty(cachedResponce) ? null : cachedResponce;
        }
        public async Task<T> Get<T>(string cacheKey) where T : class
        {
            var cachedResponce = await distributedCache.GetAsync(cacheKey);
            if (cachedResponce == null)
            {
                return null;
            }

            var strObject = Encoding.UTF8.GetString(cachedResponce);
            if (typeof(T) == typeof(string))
            {
                return strObject as T;
            }
            return JsonConvert.DeserializeObject<T>(strObject);
         
        }

    }
}
