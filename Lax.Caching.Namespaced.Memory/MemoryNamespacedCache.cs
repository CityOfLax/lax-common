using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Lax.Caching.Namespaced.Memory {

    public class MemoryNamespacedCache<TKey, TValue> : NamespacedCache<TKey, TValue> {

        private readonly IMemoryCache _memoryCache;

        private readonly TimeSpan _expiration;

        public MemoryNamespacedCache(string namespc, TimeSpan expiration, IMemoryCache memoryCache) : base(namespc) {
            _memoryCache = memoryCache;
            _expiration = expiration;
        }

        public override async Task<bool> ExistsAsync(TKey key) {
            TValue value;
            return
                await
                    Task.FromResult(
                        _memoryCache.TryGetValue(new MemoryNamespacedCacheEntryKey<TKey, TValue>(Namespc, key),
                            out value));
        }

        public override async Task<TValue> GetAsync(TKey key) =>
            await Task.FromResult(_memoryCache.Get<TValue>(new MemoryNamespacedCacheEntryKey<TKey, TValue>(Namespc, key)));

        public override async Task SetAsync(TKey key, TValue value) {
            await
                Task.FromResult(_memoryCache.Set(new MemoryNamespacedCacheEntryKey<TKey, TValue>(Namespc, key), value,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(_expiration)));
        }

        public override async Task FlushAsync(TKey key) {
            _memoryCache.Remove(new MemoryNamespacedCacheEntryKey<TKey, TValue>(Namespc, key));
            await Task.FromResult(true);
        }

    }

}
