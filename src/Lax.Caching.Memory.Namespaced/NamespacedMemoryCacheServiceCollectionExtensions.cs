using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lax.Caching.Memory.Namespaced {

    public static class NamespacedMemoryCacheServiceCollectionExtensions {

        public static IServiceCollection AddNamespacedMemoryCache(this IServiceCollection services) {

            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddMemoryCache();

            services.TryAdd(ServiceDescriptor.Transient<INamespacedMemoryCache, NamespacedMemoryCache>());

            return services;

        }

    }

}