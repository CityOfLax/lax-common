using System;

namespace Lax.Caching.Memory.Namespaced {

    public interface INamespacedMemoryCache {

        TValue Get<TKey, TValue>(string namespc, TKey key);

        void Set<TKey, TValue>(string namespc, TKey key, TValue value, TimeSpan expiration);

    }

}