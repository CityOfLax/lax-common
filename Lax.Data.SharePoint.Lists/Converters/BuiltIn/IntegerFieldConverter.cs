using System;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("Integer")]
    [SpFieldConverter("Counter")]
    [UsedImplicitly]
    internal class IntegerFieldConverter : IFieldConverter {

        private bool _isNullableMemberType;

        public void Initialize(MetaField field) {
            Guard.CheckNotNull(nameof(field), field);

            var memberType = field.MemberType;
            if (memberType.IsNullable()) {
                _isNullableMemberType = true;
                memberType = memberType.GetGenericArguments()[0];
            }
            if (memberType != typeof(int)) {
                throw new ArgumentException("Member type should be int or System.Nullable<int>.");
            }
        }

        public object FromSpValue(object value) {
            if (_isNullableMemberType) {
                return (int?) value;
            }

            return (int?) value ?? 0;
        }

        public object ToSpValue(object value) => (int?) value;

        public string ToCamlValue(object value) => Convert.ToString(ToSpValue(value));

    }

}