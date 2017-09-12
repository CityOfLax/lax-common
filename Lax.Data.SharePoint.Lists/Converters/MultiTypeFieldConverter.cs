using System;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Converters {

    /// <summary>
    /// Represents base field converter that supports multiple member types.
    /// </summary>
    [PublicAPI]
    public class MultiTypeFieldConverter : IFieldConverter {

        /// <summary>
        /// Gets model of the associated field.
        /// </summary>
        protected MetaField Field { get; private set; }

        /// <summary>
        /// Gets internal field converter that should be used with specified <see cref="MetaField.MemberType"/>.
        /// </summary>
        protected IFieldConverter Internal { get; set; }

        public virtual void Initialize(MetaField field) {
            Guard.CheckNotNull(nameof(field), field);

            Field = field;
        }

        public object FromSpValue(object value) => Internal == null
            ? throw new InvalidOperationException("This converter wasn't initialized completely")
            : Internal.FromSpValue(value);

        public object ToSpValue(object value) => Internal == null
            ? throw new InvalidOperationException("This converter wasn't initialized completely")
            : Internal.ToSpValue(value);

        public string ToCamlValue(object value) => Internal == null
            ? throw new InvalidOperationException("This converter wasn't initialized completely")
            : Internal.ToCamlValue(value);

    }

}