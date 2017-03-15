using System.Threading.Tasks;

namespace Lax.Jobs.Abstractions {

    public interface IJob<TParameters> {

        Task Run(TParameters parameters);

    }

}
