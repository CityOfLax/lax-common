using System.Threading.Tasks;

namespace Lax.DirectoryServices {

    public interface IDirectoryServices {

        Task<DirectoryServicesUser> GetUserByObjectSid(string objectSid);

    }

}