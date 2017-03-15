using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lax.Data.Repository {

    public interface ISearchRepository<TQueryModel, TResultModel> {

        Task<IEnumerable<TResultModel>> SearchAsync(TQueryModel query);

    }

}
