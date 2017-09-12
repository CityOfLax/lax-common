using System.Collections.Generic;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Configuration;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.MetaModels.Visitors;

namespace Lax.Data.SharePoint.Lists.Data {

    /// <summary>
    /// Represents interface of services that can be used inside <see cref="SpContext{TContext}"/>.
    /// </summary>
    public interface ICommonService {

        [NotNull]
        Config Config { get; }

        /// <summary>
        /// Gets ordered collection of <see cref="IMetaModel"/> processors.
        /// </summary>
        [NotNull]
        IReadOnlyCollection<IMetaModelVisitor> MetaModelProcessors { get; }

        [NotNull]
        ISpListItemsProvider GetItemsProvider([NotNull] MetaList list);

    }

}