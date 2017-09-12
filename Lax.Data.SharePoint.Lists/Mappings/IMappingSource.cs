using System;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.MetaModels.Providers;

namespace Lax.Data.SharePoint.Lists.Mappings {

    /// <summary>
    /// Represents interface that can create <see cref="MetaContext"/> and resolve list title for the specified member of this context.
    /// </summary>
    [PublicAPI]
    public interface IMappingSource : IMetaContextProvider, IListUrlResolver {

        /// <summary>
        /// Gets <see cref="Type"/> of the associated Data Context class.
        /// </summary>
        Type ContextType { get; }

    }

}