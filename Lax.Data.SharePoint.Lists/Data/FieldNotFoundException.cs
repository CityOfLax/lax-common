using System;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Data {

    /// <summary>
    /// Represents errors when <see cref="MetaField"/> wasn't found or loaded.
    /// </summary>
    public class FieldNotFoundException : Exception {

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldNotFoundException"/> class with the specified <see cref="MetaField"/>.
        /// </summary>
        /// <param name="field">Meta field that wasn't found or loaded.</param>
        public FieldNotFoundException(MetaField field)
            : base(GetMessage(field)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldNotFoundException"/> class with the specified <see cref="MetaField"/>
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="field">Meta field that wasn't found or loaded.</param>
        /// <param name="innerException">The exception that is the casue of this exception.</param>
        public FieldNotFoundException(MetaField field, Exception innerException)
            : base(GetMessage(field), innerException) { }

        private static string GetMessage(MetaField field) {
            Guard.CheckNotNull(nameof(field), field);

            return
                $"Unable to find field by internal name ${field.InternalName} in list ${field.ContentType.List.Url} that located at SP site ${field.ContentType.List.Context.Url}";
        }

    }

}