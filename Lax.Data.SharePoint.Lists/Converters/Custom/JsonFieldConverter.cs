﻿using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;
using Newtonsoft.Json;

namespace Lax.Data.SharePoint.Lists.Converters.Custom {

    /// <summary>
    /// Represents field converter that can convert JSON string to object and vice versa.
    /// </summary>
    [SpFieldConverter("_Json_")]
    public sealed class JsonFieldConverter : IFieldConverter {

        private MetaField Field { get; set; }

        /// <summary>
        /// Initialzes current instance with the specified <see cref="MetaField"/>
        /// </summary>
        /// <param name="field"></param>
        public void Initialize(MetaField field) {
            Guard.CheckNotNull(nameof(field), field);

            Field = field;
        }

        /// <summary>
        /// Converts SP field value to <see cref="MetaField.MemberType"/>.
        /// </summary>
        /// <param name="value">SP value to convert.</param>
        /// <returns>Member value.</returns>
        public object FromSpValue(object value) => string.IsNullOrEmpty((string) value)
            ? null
            : JsonConvert.DeserializeObject((string) value, Field.MemberType);

        /// <summary>
        /// Converts <see cref="MetaField.Member"/> value to SP field value.
        /// </summary>
        /// <param name="value">Member value to convert.</param>
        /// <returns>SP field value.</returns>
        public object ToSpValue(object value) => value != null ? JsonConvert.SerializeObject(value) : null;

        /// <summary>
        /// Converts <see cref="MetaField.Member"/> value to SP Caml value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Caml value.</returns>
        public string ToCamlValue(object value) => (string) ToSpValue(value);

    }

}