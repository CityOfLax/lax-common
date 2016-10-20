using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lax.Data.EntityFrameworkCore {

    public abstract class EntityFrameworkCoreRepository<TEntity, TDbContext> : IRepository<TEntity> where TEntity : class where TDbContext : DbContext {

        protected readonly EntityFrameworkCoreRepositoryService<TDbContext> EntityFrameworkCoreRepositoryService;

        protected readonly DbSet<TEntity> DbSet;

        protected EntityFrameworkCoreRepository(EntityFrameworkCoreRepositoryService<TDbContext> entityFrameworkCoreRepositoryService) {
            EntityFrameworkCoreRepositoryService = entityFrameworkCoreRepositoryService;
            DbSet = EntityFrameworkCoreRepositoryService.DbSet<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await DbSet.ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate) =>
            await DbSet.Where(predicate).ToListAsync();

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate) =>
            await DbSet.FirstOrDefaultAsync(predicate);

        public void Add(TEntity entity) {
            DbSet.Add(entity);
        }

        public void Add(IEnumerable<TEntity> entities) {
            DbSet.AddRange(entities);
        }

        public void Remove(TEntity entity) {
            DbSet.Remove(entity);
        }

        public void Remove(IEnumerable<TEntity> entities) {
            DbSet.RemoveRange(entities);
        }

    }

}