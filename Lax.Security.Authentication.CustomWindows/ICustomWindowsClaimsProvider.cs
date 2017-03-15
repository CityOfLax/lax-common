using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Lax.Security.Authentication.CustomWindows {

    public interface ICustomWindowsClaimsProvider<TUser> {

        Task<IEnumerable<Claim>> GetClaimsForUserAsync(TUser user, HttpContext context);

    }

}