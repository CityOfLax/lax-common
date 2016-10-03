using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Lax.DirectoryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Lax.Security.Claims.Ldap {

    public class LdapCustomClaimsTransformer<TUser> : ICustomClaimsTransformer<TUser> {

        public async Task<IEnumerable<Claim>> TransformUserClaimsAsync(TUser user, HttpContext context) {

            var userObjectSidProvider = context.RequestServices.GetService<IUserObjectSidProvider<TUser>>();

            var directoryServices = context.RequestServices.GetService<IDirectoryServices>();

            var directoryServicesUser =
                await directoryServices.GetUserByObjectSid(userObjectSidProvider.ObjectSidForUser(user));

            var results = new List<Claim>();

            if (directoryServicesUser != null) {

                if (directoryServicesUser.WindowsPrimarySid != null) {
                    results.Add(new Claim(ClaimTypes.PrimarySid, directoryServicesUser.WindowsPrimarySid,
                        ClaimValueTypes.Sid));
                }
                if (directoryServicesUser.EmailAddress != null) {
                    results.Add(new Claim(ClaimTypes.Email, directoryServicesUser.EmailAddress, ClaimValueTypes.Email));
                }
                if (directoryServicesUser.LastName != null) {
                    results.Add(new Claim(ClaimTypes.Surname, directoryServicesUser.LastName, ClaimValueTypes.String));
                }
                if (directoryServicesUser.FirstName != null) {
                    results.Add(new Claim(ClaimTypes.GivenName, directoryServicesUser.FirstName, ClaimValueTypes.String));
                }

            }

            return results;

        }

    }

}