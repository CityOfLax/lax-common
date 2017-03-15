using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Lax.Security.Claims {

    public interface ICustomClaimsTransformer<TUser> {

        Task<IEnumerable<Claim>> TransformUserClaimsAsync(TUser user, HttpContext context);

    }

}