using System.Threading.Tasks;

namespace Lax.Security.Authentication {

    public interface ICustomUserProvider<TUser> {

        Task<TUser> GetUserByIdentifierAsync(string identifier);

    }

}