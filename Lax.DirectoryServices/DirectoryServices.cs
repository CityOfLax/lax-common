using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Lax.DirectoryServices {

    public class DirectoryServices : IDirectoryServices {

        private readonly DirectoryServicesOptions _directoryServicesOptions;

        public DirectoryServices(IOptions<DirectoryServicesOptions> optionsAccessor) {
            _directoryServicesOptions = optionsAccessor.Value;
        }

        public async Task<DirectoryServicesUser> GetUserByObjectSid(string objectSid) => await Task.Run(() => {
            using (
                var searcher = new DirectorySearcher(new DirectoryEntry(_directoryServicesOptions.LdapConnectionString))
                ) {
                using (var entry = new DirectoryEntry(searcher.SearchRoot.Path)) {

                    searcher.Filter =
                        $"(&(objectClass=person)(objectClass=user)(objectClass=organizationalPerson)(objectSid={objectSid}))";
                    searcher.PropertiesToLoad.Add("cn");
                    searcher.PropertyNamesOnly = true;
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.Sort.Direction = SortDirection.Ascending;
                    searcher.Sort.PropertyName = "cn";
                    searcher.ReferralChasing = ReferralChasingOption.All;

                    var results = searcher.FindAll().Cast<SearchResult>().Select(sr => sr.GetDirectoryEntry()).ToList();

                    if (results.Any()) {

                        var result = results.First();

                        var directoryServicesUser = new DirectoryServicesUser {
                            WindowsAccountName = (string) result.Properties["sAMAccountName"]?.Value,
                            FirstName = (string) result.Properties["givenName"]?.Value,
                            LastName = (string) result.Properties["sn"]?.Value,
                            EmailAddress = ((string)result.Properties["mail"]?.Value),
                            WindowsPrimarySid =
                                (new SecurityIdentifier(((byte[]) result.Properties["objectSid"].Value), 0)).ToString(),
                            ParentGroupName = result.Parent.Name,
                        };

                        return directoryServicesUser;

                    }

                    return null;

                }
            }

        });

    }

}