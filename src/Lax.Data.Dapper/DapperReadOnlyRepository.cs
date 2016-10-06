using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lax.Data.Dapper {

    public class DapperReadOnlyRepository<TEntity, TDapperContext> : IReadOnlyRepository<TEntity> where TDapperContext : IDapperContext {

        private readonly TDapperContext _dapperContext;

        public DapperReadOnlyRepository(TDapperContext dapperContext) {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _dapperContext.GetSqlConnection().GetListAsync<TEntity>();


        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate) =>
            (await GetAllAsync()).Where(predicate.Compile());

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate) =>
            (await GetAllAsync()).FirstOrDefault(predicate.Compile());

    }

}
