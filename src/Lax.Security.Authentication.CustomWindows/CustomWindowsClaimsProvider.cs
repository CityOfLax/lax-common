using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lax.Caching.Memory.Namespaced;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Lax.Security.Authentication.CustomWindows {

    public class CustomWindowsClaimsProvider<TUser> : ICustomWindowsClaimsProvider<TUser> {

        private readonly CustomWindowsClaimsProviderOptions<TUser> _customWindowsClaimsProviderOptions;

        private readonly INamespacedMemoryCache _namespacedMemoryCache;

        public CustomWindowsClaimsProvider(
            IOptions<CustomWindowsClaimsProviderOptions<TUser>> customWindowsClaimsProviderOptionsAccessor,
            INamespacedMemoryCache namespacedMemoryCache) {

            _customWindowsClaimsProviderOptions = customWindowsClaimsProviderOptionsAccessor.Value;
            _namespacedMemoryCache = namespacedMemoryCache;
        }

        private async Task<IEnumerable<Claim>> GetClaimsForUserFromBackingStoresAsync(TUser user, HttpContext context)
            =>
                (await
                    Task.WhenAll(
                        _customWindowsClaimsProviderOptions.ClaimsTransformer.Select(
                            claimsTransformer => claimsTransformer.TransformUserClaimsAsync(user, context)))).SelectMany
                    (x => x);

        public async Task<IEnumerable<Claim>> GetClaimsForUserAsync(TUser user, HttpContext context) {
            var claims =
                _namespacedMemoryCache.Get<TUser, IEnumerable<Claim>>(nameof(CustomWindowsClaimsProvider<TUser>), user);

            if (claims == null) {

                claims = await GetClaimsForUserFromBackingStoresAsync(user, context);

                _namespacedMemoryCache.Set(nameof(CustomWindowsClaimsProvider<TUser>), user, claims, TimeSpan.FromHours(1.0f));

            }

            return claims;

        }
            

    }

}