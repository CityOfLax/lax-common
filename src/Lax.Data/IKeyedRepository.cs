using System.Threading.Tasks;

namespace Lax.Data {

    public interface IKeyedRepository<TEntity, TPrimaryKey> : IRepository<TEntity> {

        Task<TEntity> GetAsync(TPrimaryKey id);

    }

}