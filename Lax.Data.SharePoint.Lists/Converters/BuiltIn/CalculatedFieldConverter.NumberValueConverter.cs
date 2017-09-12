using System;
using System.Globalization;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {
    public partial class CalculatedFieldConverter : MultiTypeFieldConverter {
        private class NumberValueConverter : IFieldConverter {

            public void Initialize(MetaField field) {
                if (field.MemberType != typeof(double)) {
                    throw new ArgumentException("Only double member type is allowed.");
                }
            }

            public object FromSpValue(object value) {
                return GetValue<double>(value);
            }

            public object ToSpValue(object value) {
                return (double) value;
            }

            public string ToCamlValue(object value) {
                return ((double) value).ToString(CultureInfo.InvariantCulture);
            }

        }

    }

}