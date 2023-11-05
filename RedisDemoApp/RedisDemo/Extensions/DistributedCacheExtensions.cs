using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace RedisDemo.Extensions
{
    public static class DistributedCacheExtensions
    {
        //public int MyProperty { get; set; }

        public static async Task SetRecordAsync<T>(this IDistributedCache cache, 
            string recordId, 
            T data, 
            TimeSpan? absoluteExpireTime=null, 
            TimeSpan? unusedExpireTime = null )
        {

            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow= absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = unusedExpireTime ;

            var jasonData =  JsonSerializer.Serialize( data );
            await cache.SetStringAsync(recordId, jasonData, options);

        }


        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId )
        {

            var jasonData = await cache.GetStringAsync(recordId);

            if(jasonData== null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jasonData);


        }


    }
}
