using System.Collections.Generic;
using System.Threading.Tasks;
using Lax.Caching;

namespace Lax.Data.Repository.Cached {

    public class CachedSearchRepository<TQueryModel, TResultModel> : ISearchRepository<TQueryModel, TResultModel> {

        private readonly IAccessSearchRepository<TQueryModel, TResultModel> _accessSearchRepository;

        private readonly ICache<TQueryModel, IEnumerable<TResultModel>> _cache;

        public CachedSearchRepository(
            IAccessSearchRepository<TQueryModel, TResultModel> accessSearchRepository,
            ICache<TQueryModel, IEnumerable<TResultModel>> cache) {

            _accessSearchRepository = accessSearchRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<TResultModel>> SearchAsync(TQueryModel query) {
            if (!(await _cache.ExistsAsync(query))) {
                await _cache.SetAsync(query, await _accessSearchRepository.SearchAsync(query));
            }
            return await _cache.GetAsync(query);
        }

    }

}
