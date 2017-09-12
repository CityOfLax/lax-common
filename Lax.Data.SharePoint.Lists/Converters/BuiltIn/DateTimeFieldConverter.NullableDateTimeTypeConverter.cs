using System;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {
    internal partial class DateTimeFieldConverter : MultiTypeFieldConverter {
        private class NullableDateTimeTypeConverter : IFieldConverter {

            public void Initialize(MetaField field) { }

            public object FromSpValue(object value) => 
                ToLocalTime((DateTime?) value);

            public object ToSpValue(object value) => 
                ToLocalTime((DateTime?) value);

            public string ToCamlValue(object value) => 
                ((DateTime?)value).HasValue ? CreateIsoDate(ToLocalTime(((DateTime?)value).Value)) : "";

        }

    }

}