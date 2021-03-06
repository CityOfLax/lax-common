﻿using System.Collections.Generic;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Collections {

    /// <summary>
    /// Represents a collection of keys and values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TObject">The type of the values.</typeparam>
    [PublicAPI]
    public class Container<TKey, TObject> : IEnumerable<KeyValuePair<TKey, TObject>> {

        private readonly Dictionary<TKey, TObject> _registeredObjects = new Dictionary<TKey, TObject>();

        /// <summary>
        /// Adds or updates the specified key in collection with the specified value.
        /// </summary>
        /// <param name="key">The key of element to add or update.</param>
        /// <param name="obj">The value of element to add or update. The value cannot be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> or <paramref name="obj"/> is null.</exception>
        public void Register([NotNull] TKey key, [NotNull] TObject obj) {
            Guard.CheckNotNull(nameof(key), key);
            Guard.CheckNotNull(nameof(obj), obj);

            if (_registeredObjects.ContainsKey(key)) {
                _registeredObjects[key] = obj;
            } else {
                _registeredObjects.Add(key, obj);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The values associated with the specified key.</returns>
        /// <exception cref="KeyNotFoundException"><paramref name="key"/> was not found.</exception>
        [NotNull]
        public TObject Resolve([NotNull] TKey key) {
            Guard.CheckNotNull(nameof(key), key);

            if (_registeredObjects.ContainsKey(key)) {
                return _registeredObjects[key];
            }
            throw Error.KeyNotFound(key);
        }

        /// <summary>
        /// Determines whether the <see cref="T:Container`2"/> contains the specified key. 
        /// </summary>
        /// <param name="key">The key of the value to check.</param>
        /// <returns>true if the <see cref="T:Container`2"/> contains an element with the specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool IsRegistered([NotNull] TKey key) {
            Guard.CheckNotNull(nameof(key), key);

            return _registeredObjects.ContainsKey(key);
        }

        /// <summary>
        /// Returns the enumerator that iterates through the <see cref="T:Container`2"/>.
        /// </summary>
        /// <returns>enumerator</returns>
        public IEnumerator<KeyValuePair<TKey, TObject>> GetEnumerator() => _registeredObjects.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    }

}