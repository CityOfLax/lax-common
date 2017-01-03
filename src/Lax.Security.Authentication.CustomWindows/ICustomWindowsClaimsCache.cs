using System.Collections.Generic;
using System.Security.Claims;
using Lax.Caching;

namespace Lax.Security.Authentication.CustomWindows {

    public interface ICustomWindowsClaimsCache<TUser> : ICache<TUser, IEnumerable<Claim>> {

        

    }

}