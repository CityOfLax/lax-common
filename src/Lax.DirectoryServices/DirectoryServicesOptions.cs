using Microsoft.Extensions.Options;

namespace Lax.DirectoryServices {

    public class DirectoryServicesOptions : IOptions<DirectoryServicesOptions> {

        public string LdapConnectionString { get; set; }

        public DirectoryServicesOptions Value => this;

    }

}