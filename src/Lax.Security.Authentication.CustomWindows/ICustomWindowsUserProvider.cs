using System.Threading.Tasks;

namespace Lax.Security.Authentication.CustomWindows {

    public interface ICustomWindowsUserProvider<TUser> {

        Task<TUser> GetUserForWindowsPrimarySidAsync(string windowsPrimarySid);

    }

}