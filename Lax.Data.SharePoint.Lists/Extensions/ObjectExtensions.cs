﻿using System.Collections.Generic;
using System.Linq;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Lax.Data.SharePoint.Lists.Extensions {

    /// <summary>
    /// Provides a set of static methods for work with <see cref="object"/>.
    /// </summary>
    [PublicAPI]
    public static class ObjectExtensions {

        /// <summary>
        /// Determines whether <paramref name="collection"/> contains current object <paramref name="obj"/> or not.
        /// </summary>
        /// <typeparam name="T">The object and collection element type.</typeparam>
        /// <param name="obj">The object to check in <see cref="IEnumerable{T}"/>.</param>
        /// <param name="collection">The collection which can contain <paramref name="obj"/>.</param>
        /// <returns>true if <paramref name="obj"/> is in <paramref name="collection"/>; otherwise, false.</returns>
        public static bool In<T>([CanBeNull] this T obj, [CanBeNull] IEnumerable<T> collection) => 
            collection != null && collection.Contains(obj);

    }

}