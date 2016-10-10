using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lax.Jobs {

    public static class JobsServiceCollectionExtensions {

        public static IServiceCollection AddJobs(this IServiceCollection services) {

            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Transient<IJobExecutor, JobExecutor>());

            return services;

        }

    }

}
