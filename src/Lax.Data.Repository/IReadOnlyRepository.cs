using System.Threading.Tasks;

namespace Lax.Data.Repository {

    public interface IReadOnlyRepository<TKey, TModel> {

        Task<TModel> GetByKeyAsync(TKey key);

    }

}
