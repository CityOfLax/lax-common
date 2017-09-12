using System;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("Text")]
    [SpFieldConverter("Note")]
    [SpFieldConverter("Choice")]
    [UsedImplicitly]
    internal class TextFieldConverter : IFieldConverter {

        public void Initialize(MetaField field) {
            Guard.CheckNotNull(nameof(field), field);

            if (field.MemberType != typeof(string)) {
                throw new ArgumentException("Only string member type allowed.");
            }
        }

        public object FromSpValue(object value) => (string) value;

        public object ToSpValue(object value) => (string) value;

        public string ToCamlValue(object value) => (string) ToSpValue(value);

    }

}