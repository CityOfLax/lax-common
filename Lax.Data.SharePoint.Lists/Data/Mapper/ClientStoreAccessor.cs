using Lax.Data.SharePoint.Lists.MetaModels;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Data.Mapper {

    internal sealed class ClientStoreAccessor : StoreAccessor<ListItem> {

        public ClientStoreAccessor(MetaField field)
            : base(field) { }

        public override object GetValue(ListItem instance) => instance[Field.InternalName];

        public override void SetValue(ListItem instance, object value) {
            instance[Field.InternalName] = value;
        }

    }

}