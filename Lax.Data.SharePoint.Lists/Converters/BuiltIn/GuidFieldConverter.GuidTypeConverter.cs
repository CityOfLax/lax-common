using System;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {
    internal partial class GuidFieldConverter : MultiTypeFieldConverter {
        private class GuidTypeConverter : IFieldConverter {

            public void Initialize(MetaField field) { }

            public object FromSpValue(object value) => 
                value != null ? new Guid(value.ToString()) : Guid.Empty;

            public object ToSpValue(object value) => 
                (Guid) value;

            public string ToCamlValue(object value) => 
                ((Guid)value).ToString("D");

        }

    }

}