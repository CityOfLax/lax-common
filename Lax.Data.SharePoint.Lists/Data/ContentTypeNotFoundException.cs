using System;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Data {

    /// <summary>
    /// Represents errors when <see cref="MetaContentType"/> wasn't found or loaded.
    /// </summary>
    public class ContentTypeNotFoundException : Exception {

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeNotFoundException"/> class with the specified <see cref="MetaContentType"/>.
        /// </summary>
        /// <param name="contentType">Meta content type that wasn't found or loaded.</param>
        public ContentTypeNotFoundException(MetaContentType contentType)
            : base(GetMessage(contentType)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeNotFoundException"/> class with the specified <see cref="MetaContentType"/>
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="contentType">Meta content type that wasn't found or loaded.</param>
        /// <param name="innerException">The exception that is the casue of this exception.</param>
        public ContentTypeNotFoundException(MetaContentType contentType, Exception innerException)
            : base(GetMessage(contentType), innerException) { }

        private static string GetMessage(MetaContentType contentType) {
            Guard.CheckNotNull(nameof(contentType), contentType);

            return
                $"Unable to find or load content type ${contentType.Id} in list ${contentType.List.Url} that located at SP site ${contentType.List.Context.Url}.";
        }

    }

}