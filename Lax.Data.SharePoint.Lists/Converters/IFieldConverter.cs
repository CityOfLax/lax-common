﻿using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.MetaModels;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Lax.Data.SharePoint.Lists.Converters {

    /// <summary>
    /// Represents field converter interface.
    /// </summary>
    [PublicAPI]
    public interface IFieldConverter {

        /// <summary>
        /// Initialzes current instance with the specified <see cref="MetaField"/>
        /// </summary>
        /// <param name="field"></param>
        void Initialize([NotNull] MetaField field);

        /// <summary>
        /// Converts SP field value to <see cref="MetaField.MemberType"/>.
        /// </summary>
        /// <param name="value">SP value to convert.</param>
        /// <returns>Member value.</returns>
        [CanBeNull]
        object FromSpValue([CanBeNull] object value);

        /// <summary>
        /// Converts <see cref="MetaField.Member"/> value to SP field value.
        /// </summary>
        /// <param name="value">Member value to convert.</param>
        /// <returns>SP field value.</returns>
        [CanBeNull]
        object ToSpValue([CanBeNull] object value);

        /// <summary>
        /// Converts <see cref="MetaField.Member"/> value to SP Caml value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Caml value.</returns>
        [CanBeNull]
        string ToCamlValue([CanBeNull] object value);

    }

}