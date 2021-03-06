﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Lax.Data.SharePoint.Lists.Converters.Custom {

    /// <summary>
    /// Represents field converter that can convert string to <see cref="Enum"/> and vice versa.
    /// </summary>
    [PublicAPI]
    [SpFieldConverter("_Enum_")]
    public sealed class EnumFieldConverter : IFieldConverter {

        private bool _isNullableMemberType;
        private Type _enumType;

        /// <summary>
        /// Initialzes current instance with the specified <see cref="MetaField"/>
        /// </summary>
        /// <param name="field"></param>
        public void Initialize(MetaField field) {
            Guard.CheckNotNull(nameof(field), field);

            var memberType = field.MemberType;
            if (memberType.IsNullable()) {
                _isNullableMemberType = true;
                memberType = memberType.GetGenericArguments()[0];
            }
            if (!memberType.IsEnum) {
                throw new ArgumentException("This converter can be used only with Enum member types");
            }

            if (!Enum.IsDefined(memberType, 0)) {
                throw new ArgumentException($"Enum {field.MemberType} should have default value (i.e. 0)");
            }

            _enumType = memberType;
        }

        /// <summary>
        /// Converts SP field value to <see cref="MetaField.MemberType"/>.
        /// </summary>
        /// <param name="value">SP value to convert.</param>
        /// <returns>Member value.</returns>
        public object FromSpValue(object value) {
            if (_isNullableMemberType && value == null) {
                return null;
            }

            return ConvertToEnum(_enumType, (string) value);
        }


        /// <summary>
        /// Converts <see cref="MetaField.Member"/> value to SP field value.
        /// </summary>
        /// <param name="value">Member value to convert.</param>
        /// <returns>SP field value.</returns>
        public object ToSpValue(object value) {
            if (_isNullableMemberType && value == null) {
                return null;
            }

            return ConvertFromEnum(_enumType, value);
        }

        string IFieldConverter.ToCamlValue(object value) => (string) ToSpValue(value) ?? "";

        [NotNull]
        private static object ConvertToEnum([NotNull] Type enumType, [CanBeNull] string value) {
            if (string.IsNullOrEmpty(value)) {
                return Enum.ToObject(enumType, 0);
            }

            foreach (var enumName in Enum.GetNames(enumType)) {
                var enumMemberAttribute = enumType.GetField(enumName).GetCustomAttribute<EnumMemberAttribute>();

                if (enumMemberAttribute != null &&
                    string.Compare(enumMemberAttribute.Value, value,
                        StringComparison.InvariantCultureIgnoreCase) == 0) {
                    return Enum.Parse(enumType, enumName);
                }

                if (string.Compare(enumName, value, StringComparison.InvariantCultureIgnoreCase) == 0) {
                    return Enum.Parse(enumType, enumName);
                }
            }

            throw new InvalidEnumArgumentException("value");
        }

        private static string ConvertFromEnum([NotNull] Type enumType, [NotNull] object value) {
            var enumName = Enum.GetName(enumType, value);

            var enumMemberAttribute = enumType.GetField(enumName).GetCustomAttribute<EnumMemberAttribute>();

            return enumMemberAttribute != null ? enumMemberAttribute.Value : enumName;
        }

    }

}