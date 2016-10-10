using System.Threading.Tasks;
using Lax.Jobs.Abstractions;

namespace Lax.Jobs {

    public interface IJobExecutor {

        Task Run<TJob, TJobParameters>(TJobParameters jobParameters) where TJob : IJob<TJobParameters>;

    }

}
