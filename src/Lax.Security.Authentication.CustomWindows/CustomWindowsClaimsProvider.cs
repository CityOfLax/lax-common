using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Lax.Security.Authentication.CustomWindows {

    public class CustomWindowsClaimsProvider<TUser> : ICustomWindowsClaimsProvider<TUser> {

        private readonly CustomWindowsClaimsProviderOptions<TUser> _customWindowsClaimsProviderOptions;

        private readonly ICustomWindowsClaimsCache<TUser> _namespacedMemoryCache;

        public CustomWindowsClaimsProvider(
            IOptions<CustomWindowsClaimsProviderOptions<TUser>> customWindowsClaimsProviderOptionsAccessor,
            ICustomWindowsClaimsCache<TUser> namespacedMemoryCache) {

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
                await _namespacedMemoryCache.GetAsync(user);

            if (claims == null) {

                claims = await GetClaimsForUserFromBackingStoresAsync(user, context);

                await _namespacedMemoryCache.SetAsync(user, claims);

            }

            return claims;

        }
            

    }

}