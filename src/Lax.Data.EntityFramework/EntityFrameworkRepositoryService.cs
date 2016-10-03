using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Lax.Data.EntityFramework {

    public abstract class EntityFrameworkRepositoryService<TDbContext> : IRepositoryService, IDisposable where TDbContext : DbContext {

        private readonly TDbContext _dbContext;

        protected EntityFrameworkRepositoryService(TDbContext dbContext) {
            _dbContext = dbContext;
        }

        public DbSet<TEntity> DbSet<TEntity>() where TEntity : class =>
            _dbContext.Set<TEntity>();

        public async Task SaveChanges() {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose() {
            _dbContext?.Dispose();
        }

    }

}