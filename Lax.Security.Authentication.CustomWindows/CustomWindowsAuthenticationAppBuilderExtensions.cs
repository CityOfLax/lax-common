using Microsoft.AspNetCore.Builder;

namespace Lax.Security.Authentication.CustomWindows {

    public static class CustomWindowsAuthenticationAppBuilderExtensions {

        public static IApplicationBuilder UseCustomWindowsAuthentication<TUser>(this IApplicationBuilder builder) {
            return builder.UseMiddleware<CustomWindowsAuthenticationMiddleware<TUser>>();
        }

    }

}