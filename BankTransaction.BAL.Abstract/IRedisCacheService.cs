using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Abstract
{
    public interface IRedisCacheService
    {
        Task CacheResponce(string cacheKey, object responce, TimeSpan timeToLiveInSeconds);
        Task<string> GetCachedResponce(string cacheKey);
        Task<T> Get<T>(string cacheKey) where T : class;
        Task CacheHtmlResponce(string cacheKey, object responce, TimeSpan timeToLiveInSeconds);
    }
}
