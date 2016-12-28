using System.Threading.Tasks;
using Lax.Caching;

namespace Lax.Data.Repository.Cached {

    public class CachedReadOnlyRepository<TKey, TModel> : IReadOnlyRepository<TKey, TModel> {

        private readonly IAccessReadOnlyRepository<TKey, TModel> _accessReadOnlyRepository;

        private readonly ICache<TKey, TModel> _cache;

        public CachedReadOnlyRepository(
            IAccessReadOnlyRepository<TKey, TModel> accessReadOnlyRepository,
            ICache<TKey, TModel> cache) {

            _accessReadOnlyRepository = accessReadOnlyRepository;
            _cache = cache;
        }

        public async Task<TModel> GetByKeyAsync(TKey key) {
            if (!(await _cache.ExistsAsync(key))) {
                await _cache.SetAsync(key, await _accessReadOnlyRepository.GetByKeyAsync(key));
            }
            return await _cache.GetAsync(key);
        }

    }

}
