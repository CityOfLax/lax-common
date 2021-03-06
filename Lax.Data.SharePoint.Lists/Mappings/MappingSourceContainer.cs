﻿using System;
using Lax.Data.SharePoint.Lists.Collections;

namespace Lax.Data.SharePoint.Lists.Mappings {

    /// <summary>
    /// Represents a collection of the context types and associated <see cref="IMappingSource"/>.
    /// </summary>
    public class MappingSourceContainer : Container<Type, IMappingSource>, IMappingSourceResolver {

        /// <summary>
        /// Determines whether <paramref name="contextType"/> can be resolved or not.
        /// </summary>
        /// <param name="contextType">Context type to resolve.</param>
        /// <returns>true if <paramref name="contextType"/> can be resolved; otherwise, false.</returns>
        public bool CanResolve(Type contextType) {
            return IsRegistered(contextType);
        }

    }

}