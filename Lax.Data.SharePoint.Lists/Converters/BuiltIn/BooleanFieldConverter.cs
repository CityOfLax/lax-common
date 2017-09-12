using System;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("Boolean")]
    [UsedImplicitly]
    internal class BooleanFieldConverter : IFieldConverter {

        private bool _isNullableMemberType;

        public void Initialize(MetaField field) {
            Guard.CheckNotNull(nameof(field), field);

            var memberType = field.MemberType;
            if (memberType.IsNullable()) {
                _isNullableMemberType = true;
                memberType = memberType.GetGenericArguments()[0];
            }
            if (memberType != typeof(bool)) {
                throw new ArgumentException("Member type should be bool or Syste.Nullable<bool>.");
            }
        }

        public object FromSpValue(object value) {
            if (_isNullableMemberType) {
                return (bool?) value;
            }

            return (bool?) value ?? false;
        }

        public object ToSpValue(object value) => (bool?) value;

        public string ToCamlValue(object value) {
            var boolValue = (bool?) value;
            if (boolValue.HasValue) {
                return boolValue.Value ? "1" : "0";
            }
            return "";
        }

    }

}