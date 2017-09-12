using System.Collections.Generic;
using System.Linq;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.MetaModels.Collections;
using Lax.Data.SharePoint.Lists.MetaModels.Providers;
using Lax.Data.SharePoint.Lists.MetaModels.Visitors;
using Lax.Data.SharePoint.Lists.Utils;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Lax.Data.SharePoint.Lists.MetaModels {

    /// <summary>
    /// Represents MetaData of <see cref="ISpContext"/>
    /// </summary>
    public sealed class MetaContext : BaseMetaModel {

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaContext"/>.
        /// </summary>
        /// <param name="listProviders">Providers of <see cref="MetaList"/> associated with the current context.</param>
        /// <exception cref="ArgumentNullException"><paramref name="listProviders"/> is null.</exception>
        public MetaContext([NotNull] IReadOnlyCollection<IMetaListProvider> listProviders) {
            Guard.CheckNotNull(nameof(listProviders), listProviders);

            Lists = new MetaListCollection(listProviders.Select(n => n.GetMetaList(this)));
        }

        /// <summary>
        /// Gets collection of child <see cref="MetaList"/>.
        /// </summary>
        [NotNull]
        public MetaListCollection Lists { get; }

        /// <summary>
        /// Gets or sets SP Web Url.
        /// </summary>
        [CanBeNull]
        public string Url { get; set; }

        /// <summary>
        /// Accepts <see cref="IMetaModelVisitor"/> instance.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        public override void Accept([NotNull] IMetaModelVisitor visitor) {
            visitor.VisitContext(this);
        }

    }

}