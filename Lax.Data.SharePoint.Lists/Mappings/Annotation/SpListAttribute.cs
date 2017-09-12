using System;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Mappings.Annotation {

    /// <summary>
    /// When applied to property, specifies member that should be mapped to SP list.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    [PublicAPI]
    public sealed class SpListAttribute : Attribute {

        /// <summary>
        /// Initializes new instance of the <see cref="SpListAttribute"/>
        /// </summary>
        /// <param name="url">The site-relative URL at which the list was placed.</param>
        public SpListAttribute([NotNull] string url, string webUrl = null) {
            Guard.CheckNotNull(nameof(url), url);

            Url = url;
            WebUrl = webUrl;
        }

        /// <summary>
        /// Gets or sets the site-relative URL at which the list was placed.
        /// </summary>
        public string Url { get; }

        public string WebUrl { get; }

    }

}