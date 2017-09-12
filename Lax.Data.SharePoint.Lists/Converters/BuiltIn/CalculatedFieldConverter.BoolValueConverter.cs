using System;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {
    public partial class CalculatedFieldConverter : MultiTypeFieldConverter {
        private class BoolValueConverter : IFieldConverter {

            public void Initialize(MetaField field) {
                if (field.MemberType != typeof(bool)) {
                    throw new ArgumentException("Only bool member type is allowed.");
                }
            }

            public object FromSpValue(object value) {
                var strValue = GetValue<string>(value);
                if (string.IsNullOrEmpty(strValue)) {
                    return false;
                }

                return strValue == "1";
            }

            public object ToSpValue(object value) {
                return (bool) value ? "1" : "0";
            }

            public string ToCamlValue(object value) {
                return (bool) value ? "1" : "0";
            }

        }

    }

}