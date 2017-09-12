﻿using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Converters;
using Lax.Data.SharePoint.Lists.Mappings;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Configuration {

    /// <summary>
    /// Represents configuration that is required by <see cref="SpContext{TContext}"/>.
    /// </summary>
    [PublicAPI]
    public sealed class Config {

        /// <summary>
        /// Intializes a new instance of the <see cref="Config"/> with the specified instances of <see cref="IFieldConverterResolver"/> and <see cref="IMappingSourceResolver"/>.
        /// </summary>
        /// <param name="fieldConverters">Field converters resolver.</param>
        /// <param name="mappings">Mappings source resolvers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fieldConverters"/> or <paramref name="mappings"/> is null.</exception>
        public Config([NotNull] IFieldConverterResolver fieldConverters, [NotNull] IMappingSourceResolver mappings) {
            Guard.CheckNotNull(nameof(fieldConverters), fieldConverters);
            Guard.CheckNotNull(nameof(mappings), mappings);

            FieldConverters = fieldConverters;
            Mappings = mappings;
        }


        /// <summary>
        /// Gets the <see cref="IFieldConverterResolver"/> resolver.
        /// </summary>
        [NotNull]
        public IFieldConverterResolver FieldConverters { get; }

        /// <summary>
        /// Gets the <see cref="IMappingSourceResolver"/> resolver.
        /// </summary>
        [NotNull]
        public IMappingSourceResolver Mappings { get; }

    }

}