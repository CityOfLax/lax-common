using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Lax.Security.Authentication.CustomWindows {

    public class CustomWindowsUserProvider<TUser> : ICustomWindowsUserProvider<TUser> {

        private readonly ICustomUserProvider<TUser> _customUserProvider;

        private readonly IMemoryCache _userCache;

        public CustomWindowsUserProvider(
            ICustomUserProvider<TUser> customUserProvider,
            IMemoryCache userCache) {

            _customUserProvider = customUserProvider;
            _userCache = userCache;
        }

        public async Task<TUser> GetUserForWindowsPrimarySidAsync(string windowsPrimarySid) {

            var cachedUser = _userCache.Get<TUser>(windowsPrimarySid);

            if (cachedUser == null) {

                cachedUser = await _customUserProvider.GetUserByIdentifierAsync(windowsPrimarySid);

                if (cachedUser != null) {

                    _userCache.Set(windowsPrimarySid, cachedUser);

                }

            }

            return cachedUser;

        }

    }

}