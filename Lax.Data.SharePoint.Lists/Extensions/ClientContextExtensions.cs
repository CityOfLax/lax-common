using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Extensions {

    internal static class ClientContextExtensions {

        /// <summary>
        /// Returns <see cref="List"/> by server-relative list URL.
        /// </summary>
        /// <param name="context">Current <see cref="ClientContext"/>.</param>
        /// <param name="listUrl">The site-relative list URL.</param>
        /// <returns></returns>
        public static List GetListByUrl(this ClientContext context, string listUrl, string webUrl = null) {
            

            if (webUrl != null) {
                var web = context.Site.OpenWeb(webUrl);
                context.Load(web);
                context.ExecuteQuery();
                var serverRelativeUrl = context.Url.TrimEnd('/') + "/" + webUrl.TrimEnd('/') + "/" + listUrl.TrimStart('/');
                var list = web.GetList(serverRelativeUrl);
                context.Load(list, l => l, l => l.Fields, l => l.ContentTypes);
                context.ExecuteQuery();

                return list;
            } else {
                var serverRelativeUrl = context.Url.TrimEnd('/') + "/" + listUrl.TrimStart('/');
                var list = context.Web.GetList(serverRelativeUrl);
                context.Load(list, l => l, l => l.Fields, l => l.ContentTypes);
                context.ExecuteQuery();

                return list;
            }

            
        }

    }

}