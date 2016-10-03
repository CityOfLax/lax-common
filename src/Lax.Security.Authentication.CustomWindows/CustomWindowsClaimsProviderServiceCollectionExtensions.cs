using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lax.Security.Authentication.CustomWindows {

    public static class CustomWindowsClaimsProviderServiceCollectionExtensions {

        public static IServiceCollection AddCustomWindowsClaimsProvider<TUser>(this IServiceCollection services) {

            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddMemoryCache();
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Transient<ICustomWindowsClaimsProvider<TUser>, CustomWindowsClaimsProvider<TUser>>());

            return services;

        }

        public static IServiceCollection AddCustomWindowsClaimsProvider<TUser>(
            this IServiceCollection services,
            Action<CustomWindowsClaimsProviderOptions<TUser>> setupAction) {

            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null) {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddCustomWindowsClaimsProvider<TUser>();
            services.Configure(setupAction);

            return services;

        }

    }

}