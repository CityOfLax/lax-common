using System;
using Microsoft.Extensions.Caching.Memory;

namespace Lax.Caching.Memory.Namespaced {

    public class NamespacedMemoryCache : INamespacedMemoryCache {

        private readonly IMemoryCache _memoryCache;

        public NamespacedMemoryCache(IMemoryCache memoryCache) {
            _memoryCache = memoryCache;
        }

        public TValue Get<TKey, TValue>(string namespc, TKey key) =>
            _memoryCache.Get<TValue>(new NamespacedMemoryCacheEntryKey<TKey, TValue>(namespc, key));

        public void Set<TKey, TValue>(string namespc, TKey key, TValue value, TimeSpan expiration) {
            _memoryCache.Set(new NamespacedMemoryCacheEntryKey<TKey, TValue>(namespc, key), value,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiration));
        }

    }

}