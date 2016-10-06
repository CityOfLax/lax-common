using System.Collections.Generic;

namespace Lax.Data {

    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> {

        void Add(TEntity entity);

        void Add(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void Remove(IEnumerable<TEntity> entities);

    }

}