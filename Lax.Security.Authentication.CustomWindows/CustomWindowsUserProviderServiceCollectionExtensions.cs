﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lax.Security.Authentication.CustomWindows {

    public static class CustomWindowsUserProviderServiceCollectionExtensions {

        public static IServiceCollection AddCustomWindowsUserProvider<TUser>(this IServiceCollection services) {

            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }
            
            services.TryAdd(ServiceDescriptor.Transient<ICustomWindowsUserProvider<TUser>, CustomWindowsUserProvider<TUser>>());

            return services;

        }

    }

}