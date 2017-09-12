using Lax.Data.SharePoint.Lists.MetaModels;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    internal partial class UrlFieldConverter : MultiTypeFieldConverter {

        private class StringTypeConverter : IFieldConverter {

            public void Initialize(MetaField field) { }

            public object FromSpValue(object value) => ((FieldUrlValue) value)?.Url;

            public object ToSpValue(object value) => value == null ? null : new FieldUrlValue {Url = value.ToString()};

            public string ToCamlValue(object value) => (string) value ?? "";

        }

    }

}