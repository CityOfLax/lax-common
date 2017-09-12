using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Mappings;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Data {

    /// <summary>
    /// Represents interface for SP data context types.
    /// </summary>
    [PublicAPI]
    public interface ISpContext {

        /// <summary>
        /// Gets <see cref="IMappingSource"/> that is used by this instance of the <see cref="ISpContext"/>
        /// </summary>
        [NotNull]
        IMappingSource MappingSource { get; }

        /// <summary>
        /// Gets <see cref="MetaContext"/> that is used by this instance of the <see cref="ISpContext"/>
        /// </summary>
        [NotNull]
        MetaContext Model { get; }

    }

}