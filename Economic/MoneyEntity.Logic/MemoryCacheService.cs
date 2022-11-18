using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntity.Logic
{
    public class MemoryCacheService
    {
        MemoryCache _memoryCache;
        public MemoryCacheService(MemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool TryGetValue<TCache>(string key, out TCache? result)
        {
            return _memoryCache.TryGetValue(key, out result);
        }

        public async Task<TCache> GetOrCreateCacheList<TCache>(Func<Task<TCache>> loadFunc, bool sliding, TimeSpan span)
        {
            var id = $"list";
            var type = typeof(TCache);
            var key = $"{id}_{type}";
            return await GetOrCreateCacheAsync(async () => await loadFunc(), key, sliding, span);
        }

        public async Task<TCache> GetOrCreateCacheAsync<TCache>(Func<Task<TCache>> update, string key, bool sliding, TimeSpan span)
        {
            TCache cacheEntry;

            cacheEntry = await _memoryCache.GetOrCreateAsync(key, entry =>
            {
                if (sliding)
                    entry.SlidingExpiration = span;
                else
                    entry.AbsoluteExpiration = DateTimeOffset.Now.Add(span);
                return update();
            });
            return cacheEntry;
        }

        public TCache UpdateOrCreateCacheAsync<TCache>(string key, TCache item, bool sliding, TimeSpan span)
        {
            return sliding ?
                _memoryCache.Set(key, item, span) :
                _memoryCache.Set(key, item, DateTimeOffset.Now.Add(span));
        }

    }
}
