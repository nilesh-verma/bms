using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BMS.Models
{
    public static class RedisCache
    {
        public static async Task<IList<Movie>> GetSearchResultAsync(this IDistributedCache cache, string searchId)
        {
            return await cache.GetAsync<IList<Movie>>(searchId);
        }

        public static async Task AddSearchResultsAsync(this IDistributedCache cache, string searchId, IList<Movies> backPacks, int sec)
        {
            var options = new DistributedCacheEntryOptions();

            options.SlidingExpiration = TimeSpan.FromSeconds(sec);
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(sec);
            await cache.SetAsync(searchId, backPacks, options);
        }

        public static async Task<List<Movie>> GetAsync<T>(this IDistributedCache cache, string key) where T : class
        {
            var json = await cache.GetStringAsync(key);
            List<Movie> ls = new List<Movie>();
            if (json == null)
                return null;

            List<Movie> results = JsonConvert.DeserializeAnonymousType<List<Movie>>(json, ls);

            return results;
        }
        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options) where T : class
        {
            var json = JsonConvert.SerializeObject(value);
            await cache.SetStringAsync(key, json, options);
        }
    }
}
