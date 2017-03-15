using System.Threading.Tasks;
using Lax.Jobs.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Lax.Jobs {

    public class JobExecutor : IJobExecutor {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public JobExecutor(IServiceScopeFactory serviceScopeFactory) {

            _serviceScopeFactory = serviceScopeFactory;

        }

        public async Task Run<TJob, TJobParameters>(TJobParameters jobParameters) where TJob : IJob<TJobParameters> {
            using (var serviceScope = _serviceScopeFactory.CreateScope()) {
                var job = (IJob<TJobParameters>)ActivatorUtilities.CreateInstance<TJob>(serviceScope.ServiceProvider);
                await job.Run(jobParameters);
            }
        }

    }

}
