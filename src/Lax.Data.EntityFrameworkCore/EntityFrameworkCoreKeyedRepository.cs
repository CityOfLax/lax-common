using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lax.Data.EntityFrameworkCore {

    public class EntityFrameworkCoreKeyedRepository<TEntity, TPrimaryKey, TDbContext> : EntityFrameworkCoreRepository<TEntity, TDbContext>, IKeyedRepository<TEntity, TPrimaryKey> where TDbContext : DbContext where TEntity : class {

        public EntityFrameworkCoreKeyedRepository(EntityFrameworkCoreRepositoryService<TDbContext> entityFrameworkCoreRepositoryService) : base(entityFrameworkCoreRepositoryService) {}

        public async Task<TEntity> GetAsync(TPrimaryKey id) =>
            await DbSet.FindAsync(id);

    }

}