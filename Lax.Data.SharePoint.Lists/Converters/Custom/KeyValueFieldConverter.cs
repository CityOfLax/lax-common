﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Converters.Custom {

    /// <summary>
    /// Represetns field converter that can convert string to <see cref="Dictionary{String,String}"/> and vice versa.
    /// This converter use next notation for string: Key1 : Value1 ;  Key2 : Value2
    /// </summary>
    [SpFieldConverter("_KeyValue_")]
    public class KeyValueFieldConverter : IFieldConverter {

        private const string PairDelimiter = ";";
        private const string KeyValueDelimiter = ":";

        /// <summary>
        /// Initialzes current instance with the specified <see cref="MetaField"/>
        /// </summary>
        /// <param name="field"></param>
        public void Initialize(MetaField field) {
            Guard.CheckNotNull(nameof(field), field);

            if (!field.MemberType.IsAssignableFrom(typeof(Dictionary<string, string>))) {
                throw new ArgumentException("Member type should be assignable from System.Dictionary<string, string>.");
            }
        }

        /// <summary>
        /// Converts SP field value to <see cref="MetaField.MemberType"/>.
        /// </summary>
        /// <param name="value">SP value to convert.</param>
        /// <returns>Member value.</returns>
        public object FromSpValue(object value) {
            if (value == null) {
                return null;
            }

            var collection = new Dictionary<string, string>();

            ((string) value)
                .Split(new[] {PairDelimiter}, StringSplitOptions.RemoveEmptyEntries)
                .Select(SplitKeyValue)
                .Where(n => n.Length > 0)
                .Each(n => collection.Add(n[0], Enumerable.ElementAtOrDefault<string>(n, 1)));

            return collection;
        }

        private static string[] SplitKeyValue(string str) => 
            str.Split(new[] {KeyValueDelimiter}, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.Trim())
                .ToArray();

        /// <summary>
        /// Converts <see cref="MetaField.Member"/> value to SP field value.
        /// </summary>
        /// <param name="value">Member value to convert.</param>
        /// <returns>SP field value.</returns>
        public object ToSpValue(object value) => 
            ((IEnumerable<KeyValuePair<string, string>>) value)
                ?.Select(n => $"{n.Key}{KeyValueDelimiter}{n.Value}")
                .JoinToString(PairDelimiter);

        /// <summary>
        /// Converts <see cref="MetaField.Member"/> value to SP Caml value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Caml value.</returns>
        public string ToCamlValue(object value) => 
            (string) ToSpValue(value) ?? "";

    }

}