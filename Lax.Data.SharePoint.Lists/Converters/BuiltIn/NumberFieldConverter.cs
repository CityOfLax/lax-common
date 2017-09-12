using System;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("Number")]
    [SpFieldConverter("Currency")]
    [UsedImplicitly]
    internal class NumberFieldConverter : IFieldConverter {

        private bool _isNullableMemberType;

        public void Initialize(MetaField field) {
            Guard.CheckNotNull(nameof(field), field);

            var memberType = field.MemberType;
            if (memberType.IsNullable()) {
                _isNullableMemberType = true;
                memberType = memberType.GetGenericArguments()[0];
            }
            if (memberType != typeof(double)) {
                throw new ArgumentException("Member type should be double or System.Nullable<double>.");
            }
        }

        public object FromSpValue(object value) {
            if (_isNullableMemberType) {
                return (double?) value;
            }

            return (double?) value ?? 0;
        }

        public object ToSpValue(object value) => (double?) value;

        public string ToCamlValue(object value) => Convert.ToString(ToSpValue(value));

    }

}