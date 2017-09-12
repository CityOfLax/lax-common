using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Data {

    internal partial class SpListItemsProvider : BaseSpListItemsProvider<ListItem> {
        private class UpdateItemInfo {

            public UpdateItemInfo([NotNull] ListItem spItem, [NotNull] object item) {
                SpItem = spItem;
                Item = item;
            }

            [NotNull]
            public ListItem SpItem { get; }

            [NotNull]
            public object Item { get; }

        }

    }

}