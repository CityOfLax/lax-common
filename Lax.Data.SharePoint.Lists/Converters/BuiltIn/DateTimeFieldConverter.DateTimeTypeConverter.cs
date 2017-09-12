using System;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {
    internal partial class DateTimeFieldConverter : MultiTypeFieldConverter {
        private class DateTimeTypeConverter : IFieldConverter {

            private static readonly DateTime Min = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local);

            public void Initialize(MetaField field) { }

            public object FromSpValue(object value) => 
                ToLocalTime((DateTime?) value ?? Min);

            public object ToSpValue(object value) => 
                ToLocalTime((DateTime)value) > Min ? ToLocalTime((DateTime)value) : (object) null;

            public string ToCamlValue(object value) => 
                (DateTime)value > Min ? CreateIsoDate(ToLocalTime((DateTime)value)) : "";

        }

    }

}