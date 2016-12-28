using System;
using System.Threading.Tasks;

namespace Lax.Caching {

    public interface ICache<TKey, TValue> {

        Task<bool> ExistsAsync(TKey key);

        Task<TValue> GetAsync(TKey key);

        Task SetAsync(TKey key, TValue value, TimeSpan expiration);

        Task FlushAsync(TKey key);

    }

}
