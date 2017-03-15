using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Lax.Security.Authentication.CustomWindows {

    public class CustomWindowsAuthenticationMiddleware<TUser> {

        private readonly ICustomWindowsUserProvider<TUser> _customWindowsUserProvider;

        private readonly ICustomWindowsClaimsProvider<TUser> _customWindowsClaimsProvider;

        private readonly RequestDelegate _next;

        public CustomWindowsAuthenticationMiddleware(
            RequestDelegate next,
            ICustomWindowsUserProvider<TUser> customWindowsUserProvider,
            ICustomWindowsClaimsProvider<TUser> customWindowsClaimsProvider) {

            _next = next;
            _customWindowsUserProvider = customWindowsUserProvider;
            _customWindowsClaimsProvider = customWindowsClaimsProvider;
        }

        public async Task Invoke(HttpContext context) {

            if (context.User.Identity.IsAuthenticated && context.User.HasClaim(claim => claim.Type.Equals(ClaimTypes.PrimarySid))) {

                var primarySid = context.User.Claims.First(claim => claim.Type.Equals(ClaimTypes.PrimarySid)).Value;

                var user = await _customWindowsUserProvider.GetUserForWindowsPrimarySidAsync(primarySid);

                if (user != null) {

                    context.User =
                        new ClaimsPrincipal(
                            new ClaimsIdentity(await _customWindowsClaimsProvider.GetClaimsForUserAsync(user, context),
                                "CustomWindowsAuthentication"));

                    await _next.Invoke(context);
                    return;

                }

            }

            context.Response.StatusCode = 401;

        }


    }

}