using System;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {
    public partial class CalculatedFieldConverter : MultiTypeFieldConverter {
        private class DateTimeValueConverter : IFieldConverter {

            public void Initialize(MetaField field) {
                if (field.MemberType != typeof(DateTime)) {
                    throw new ArgumentException("Only DateTime member type is allowed.");
                }
            }

            public object FromSpValue(object value) {
                return GetValue<DateTime>(value);
            }

            public object ToSpValue(object value) {
                return (DateTime) value;
            }

            public string ToCamlValue(object value) {
                return ((DateTime) value).ToString("u");
            }

        }

    }

}