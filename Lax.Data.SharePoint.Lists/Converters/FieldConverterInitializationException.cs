using System;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Converters {

    /// <summary>
    /// Represents errors that occurs during <see cref="IFieldConverter"/> initialization.
    /// </summary>
    public class FieldConverterInitializationException : FieldConverterException {

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldConverterInitializationException"/> class with the specified <see cref="IFieldConverter"/> type
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="converterType">Field converter type that is the cause of this error.</param>
        /// <param name="field">Field that is the cause of this exception.</param>
        /// <param name="innerException">The exception that is the casue of this exception.</param>
        public FieldConverterInitializationException(Type converterType, MetaField field, Exception innerException)
            : base(GetMessage(converterType, field), innerException) { }

        private static string GetMessage(Type converterType, MetaField field) => 
            $"Field converter '{converterType}' cannot be initialized for field '{field?.Member}'";

    }

}