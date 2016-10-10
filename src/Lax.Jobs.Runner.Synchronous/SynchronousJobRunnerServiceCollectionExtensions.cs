using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lax.Jobs.Runner.Synchronous {

    public static class SynchronousJobRunnerServiceCollectionExtensions {

        public static IServiceCollection AddSynchronousJobRunner(this IServiceCollection services) {

            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Transient<IJobRunner, SynchronousJobRunner>());

            return services;

        }

    }

}
