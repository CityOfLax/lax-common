using System;
using System.Threading.Tasks;

namespace Lax.Caching.Namespaced {

    public abstract class NamespacedCache<TKey, TValue> : ICache<TKey, TValue> {

        protected readonly string Namespc;

        protected NamespacedCache(string namespc) {
            Namespc = namespc;
        }

        public abstract Task<bool> ExistsAsync(TKey key);

        public abstract Task<TValue> GetAsync(TKey key);

        public abstract Task SetAsync(TKey key, TValue value, TimeSpan expiration);

        public abstract Task FlushAsync(TKey key);

    }

}
