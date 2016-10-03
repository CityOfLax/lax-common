using System;
using System.Threading.Tasks;
using Lax.Caching.Memory.Namespaced;

namespace Lax.Security.Authentication.CustomWindows {

    public class CustomWindowsUserProvider<TUser> : ICustomWindowsUserProvider<TUser> {

        private readonly ICustomUserProvider<TUser> _customUserProvider;

        private readonly INamespacedMemoryCache _namespacedMemoryCache;

        public CustomWindowsUserProvider(
            ICustomUserProvider<TUser> customUserProvider,
            INamespacedMemoryCache namespacedMemoryCache) {

            _customUserProvider = customUserProvider;
            _namespacedMemoryCache = namespacedMemoryCache;
        }

        public async Task<TUser> GetUserForWindowsPrimarySidAsync(string windowsPrimarySid) {

            var cachedUser = _namespacedMemoryCache.Get<string, TUser>(nameof(CustomWindowsUserProvider<TUser>),
                windowsPrimarySid);

            if (cachedUser == null) {

                cachedUser = await _customUserProvider.GetUserByIdentifierAsync(windowsPrimarySid);

                if (cachedUser != null) {
                    
                    _namespacedMemoryCache.Set(nameof(CustomWindowsUserProvider<TUser>), windowsPrimarySid, cachedUser, TimeSpan.FromHours(1.0f));

                }

            }

            return cachedUser;

        }

    }

}