using Lax.Data.SharePoint.Lists.MetaModels;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Data.Mapper {

    internal sealed class ClientTypeMapper : TypeMapper<ListItem> {

        public ClientTypeMapper(MetaContentType contentType)
            : base(contentType) { }

        protected override void SetContentType(ListItem spItem) {
            spItem["ContentTypeId"] = ContentType.Id;
        }

    }

}