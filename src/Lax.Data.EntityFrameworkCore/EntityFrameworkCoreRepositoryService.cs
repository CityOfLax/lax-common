using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lax.Data.EntityFrameworkCore {

    public abstract class EntityFrameworkCoreRepositoryService<TDbContext> : IRepositoryService, IDisposable where TDbContext : DbContext {

        private readonly TDbContext _dbContext;

        protected EntityFrameworkCoreRepositoryService(TDbContext dbContext) {
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