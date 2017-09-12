using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Utils {

    internal static class CamlUtility {

        internal static CamlQuery CamlStringToSPQuery(string caml) => 
            new CamlQuery {
                ViewXml = caml
            };

    }

}