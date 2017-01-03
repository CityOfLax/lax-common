using System;
using System.Threading.Tasks;

namespace Lax.Security.Authentication.CustomWindows {

    public class CustomWindowsUserProvider<TUser> : ICustomWindowsUserProvider<TUser> {

        private readonly ICustomUserProvider<TUser> _customUserProvider;

        private readonly ICustomWindowsUserCache<TUser> _userCache;

        public CustomWindowsUserProvider(
            ICustomUserProvider<TUser> customUserProvider,
            ICustomWindowsUserCache<TUser> userCache) {

            _customUserProvider = customUserProvider;
            _userCache = userCache;
        }

        public async Task<TUser> GetUserForWindowsPrimarySidAsync(string windowsPrimarySid) {

            var cachedUser = await _userCache.GetAsync(windowsPrimarySid);

            if (cachedUser == null) {

                cachedUser = await _customUserProvider.GetUserByIdentifierAsync(windowsPrimarySid);

                if (cachedUser != null) {

                    await _userCache.SetAsync(windowsPrimarySid, cachedUser);

                }

            }

            return cachedUser;

        }

    }

}