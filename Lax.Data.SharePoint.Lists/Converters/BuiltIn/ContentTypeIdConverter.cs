using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("ContentTypeId")]
    [UsedImplicitly]
    internal class ContentTypeIdConverter : IFieldConverter {

        public void Initialize(MetaField field) {
            Guard.CheckNotNull("field", field);
        }

        public object FromSpValue(object value) => value?.ToString();

        public object ToSpValue(object value) => (string) value;

        public string ToCamlValue(object value) => (string) value;

    }

}