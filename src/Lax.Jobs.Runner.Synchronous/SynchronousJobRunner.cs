using Lax.Jobs.Abstractions;

namespace Lax.Jobs.Runner.Synchronous {

    public class SynchronousJobRunner : IJobRunner {

        private readonly IJobExecutor _jobExecutor;

        public SynchronousJobRunner(IJobExecutor jobExecutor) {
            _jobExecutor = jobExecutor;
        }

        public void Run<TJob, TJobParameters>(TJobParameters jobParameters) where TJob : IJob<TJobParameters> {
            _jobExecutor.Run<TJob, TJobParameters>(jobParameters).GetAwaiter().GetResult();
        }

    }

}
