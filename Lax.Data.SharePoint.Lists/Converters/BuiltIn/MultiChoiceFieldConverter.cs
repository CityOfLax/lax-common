using System;
using System.Collections.Generic;
using System.Linq;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("MultiChoice")]
    [UsedImplicitly]
    internal class MultiChoiceFieldConverter : IFieldConverter {

        private bool _isArray;

        public void Initialize(MetaField field) {
            Guard.CheckNotNull("field", field);

            if (field.MemberType != typeof(string[]) &&
                !field.MemberType.IsAssignableFrom(typeof(List<string>))) {
                throw new ArgumentException(
                    "Only string[] or any class assignable from List<string> can be used as a member type.");
            }

            _isArray = field.MemberType.IsArray;
        }

        public object FromSpValue(object value) {
            if (value == null) {
                return null;
            }

            return _isArray ? (object)((IEnumerable<string>)value).Distinct().ToArray() : ((IEnumerable<string>)value).Distinct().ToList();
        }

        public object ToSpValue(object value) => 
            ((IEnumerable<string>)value)?.ToList();

        public string ToCamlValue(object value) {
            if (value == null) {
                return "";
            }

            var singleValue = value as string;
            if (singleValue != null) {
                return $";#{singleValue};#";
            }

            var multiValue = ((IEnumerable<string>) value).Distinct().ToList();
            return multiValue.Any() ? $";#{multiValue.JoinToString(";#")};#" : "";
        }

    }

}