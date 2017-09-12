using System;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {
    public partial class CalculatedFieldConverter : MultiTypeFieldConverter {
        private class TextValueConverter : IFieldConverter {

            public void Initialize(MetaField field) {
                if (field.MemberType != typeof(string)) {
                    throw new ArgumentException("Only string member type is allowed.");
                }
            }

            public object FromSpValue(object value) {
                return GetValue<string>(value);
            }

            public object ToSpValue(object value) {
                return (string) value;
            }

            public string ToCamlValue(object value) {
                return (string) value;
            }

        }

    }

}