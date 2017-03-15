using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lax.Data.EntityFrameworkCore {

    public abstract class EntityFrameworkCoreReadOnlyRepository<TEntity, TDbContext> : IReadOnlyRepository<TEntity> where TDbContext : DbContext where TEntity : class {

        protected readonly EntityFrameworkCoreRepositoryService<TDbContext> EntityFrameworkCoreRepositoryService;

        protected readonly DbSet<TEntity> DbSet;

        protected EntityFrameworkCoreReadOnlyRepository(
            EntityFrameworkCoreRepositoryService<TDbContext> entityFrameworkCoreRepositoryService) {

            EntityFrameworkCoreRepositoryService = entityFrameworkCoreRepositoryService;
            DbSet = EntityFrameworkCoreRepositoryService.DbSet<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await DbSet.ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
            => await DbSet.Where(predicate).ToListAsync();

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
            => await DbSet.FirstOrDefaultAsync(predicate);

    }

}