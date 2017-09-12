using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.MetaModels.Visitors;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Data.Mapper {

    internal class MapperInitializer : BaseMetaModelVisitor {

        public override void VisitContentType(MetaContentType contentType) {
            contentType.SetMapper(new ClientTypeMapper(contentType));

            base.VisitContentType(contentType);
        }

        public override void VisitField(MetaField field) {
            field.SetMapper(new FieldMapper<ListItem>(field, new ClientStoreAccessor(field)));
        }

    }

}