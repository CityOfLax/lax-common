using System.Data.Entity;
using System.Threading.Tasks;

namespace Lax.Data.EntityFramework {

    public abstract class EntityFrameworkKeyedRepository<TEntity, TPrimaryKey, TDbContext> : EntityFrameworkRepository<TEntity, TDbContext>, IKeyedRepository<TEntity, TPrimaryKey> where TEntity : class where TDbContext : DbContext {

        protected EntityFrameworkKeyedRepository(EntityFrameworkRepositoryService<TDbContext> permitDetailsReportSetupRepositoryService) :
            base(permitDetailsReportSetupRepositoryService) { }

        public async Task<TEntity> GetAsync(TPrimaryKey id) =>
            await DbSet.FindAsync(id);

    }

}