using Lax.Jobs.Abstractions;

namespace Lax.Jobs.Runner {

    public interface IJobRunner {

        void Run<TJob, TJobParameters>(TJobParameters jobParameters) where TJob : IJob<TJobParameters>;

    }

}
