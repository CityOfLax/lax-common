using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lax.DirectoryServices {

    public static class DirectoryServicesServiceCollectionExtensions {

        public static IServiceCollection AddDirectoryServices(this IServiceCollection services) {

            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Transient<IDirectoryServices, DirectoryServices>());

            return services;

        }

        public static IServiceCollection AddDirectoryServices(
            this IServiceCollection services,
            Action<DirectoryServicesOptions> setupAction) {

            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null) {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddDirectoryServices();
            services.Configure(setupAction);

            return services;

        }
        
    }

}