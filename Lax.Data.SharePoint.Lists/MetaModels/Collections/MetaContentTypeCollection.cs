using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.MetaModels.Collections {

    /// <summary>
    /// Represents collection of <see cref="MetaContentType"/> with fast access by <see cref="MetaContentType.EntityType"/>.
    /// </summary>
    public sealed class MetaContentTypeCollection : ReadOnlyDictionary<Type, MetaContentType>,
        IReadOnlyCollection<MetaContentType> {

        /// <summary>
        /// Initialize new instance of <see cref="MetaContentType"/> class around <paramref name="source"/>.
        /// </summary>
        /// <param name="source">Collection of <see cref="MetaContentType"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public MetaContentTypeCollection([NotNull] [ItemNotNull] IEnumerable<MetaContentType> source)
            : base(CreateDictionary(source)) { }

        IEnumerator<MetaContentType> IEnumerable<MetaContentType>.GetEnumerator() => Values.GetEnumerator();

        [NotNull]
        private static IDictionary<Type, MetaContentType> CreateDictionary(
            [NotNull] [ItemNotNull] IEnumerable<MetaContentType> source) {
            Guard.CheckNotNull(nameof(source), source);

            return source.ToDictionary(n => n.EntityType);
        }

    }

}