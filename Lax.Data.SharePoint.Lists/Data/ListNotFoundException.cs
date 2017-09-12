using System;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Data {

    /// <summary>
    /// Represents errors when <see cref="MetaList"/> wasn't found or loaded.
    /// </summary>
    public class ListNotFoundException : Exception {

        /// <summary>
        /// Initializes a new instance of the <see cref="ListNotFoundException"/> class with the specified <see cref="MetaList"/>.
        /// </summary>
        /// <param name="list">Meta list that wasn't found or loaded.</param>
        public ListNotFoundException(MetaList list)
            : base(GetMessage(list)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListNotFoundException"/> class with the specified <see cref="MetaList"/>
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="list">Meta list that wasn't found or loaded.</param>
        /// <param name="innerException">The exception that is the casue of this exception.</param>
        public ListNotFoundException(MetaList list, Exception innerException)
            : base(GetMessage(list), innerException) { }

        private static string GetMessage(MetaList list) {
            Guard.CheckNotNull(nameof(list), list);

            return $"Unable to find or load list by url ${list.Url} that located in SP site ${list.Context.Url}";
        }

    }

}