using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.Converters;
using Lax.Data.SharePoint.Lists.Data;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Utils {

    internal static class Error {

        internal static Exception KeyNotFound(object key) => 
            new KeyNotFoundException($"Key not found '{key}'");

        public static Exception MoreThanOneMatch() => 
            new InvalidOperationException("More than one match found");

        public static Exception NoMatch() => 
            new InvalidOperationException("No match found");

        internal static Exception SubqueryNotSupported(Expression node) => 
            new NotSupportedException($"Subquery '{node}' is not supported");

        internal static Exception CannotMapFieldToSP(MetaField field, Exception inner) => 
            new DataMappingException($"Cannot map member '{field.MemberName}' of type '{field.MemberType}' to SP field {field.InternalName}.", inner);

        internal static Exception CannotMapFieldFromSP(MetaField field, Exception inner) => 
            new DataMappingException($"Cannot map member '{field.MemberName}' of type '{field.MemberType}' from SP field {field.InternalName}.", inner);

        internal static Exception OperationNotAllowedForExternalList() => 
            new InvalidOperationException("This operation cannot be used with external list");

        internal static Exception OperationRequireIdField() => 
            new InvalidOperationException("This operation require ID field");

        internal static Exception CannotConvertFromSpValue(Type converterType, object spValue, Exception inner) => 
            new FieldConverterException($"SP value '{spValue}' cannot be converted by '{converterType}' field converter", inner);

        internal static Exception CannotConvertToSpValue(Type converterType, object value, Exception inner) =>
            new FieldConverterException($"Field converter '{converterType}' cannot convert value '{value}' to SP value", inner);

        internal static Exception CannotConvertToCamlValue(Type converterType, object value, Exception inner) => 
            new FieldConverterException($"Field converter '{converterType}' cannot convert value '{value}' to CAML value", inner);

        internal static Exception SubqueryNotSupportedAfterProjection(MethodCallExpression node) {

            throw new NotSupportedException($"Method '.{node.Method.Name}({GetArgs(node.Arguments)})' cannot be applied after any projection method like '.Select'");
        }

        internal static Exception SubqueryNotSupportedAfterRowLimit(MethodCallExpression node) {

            throw new NotSupportedException($"Method '.{node.Method.Name}({GetArgs(node.Arguments)})' cannot be applied after any row limit method like '.Take'");
        }

        internal static Exception ConverterNotFound(string typeAsString) =>
            new FieldConverterException($"Cannot find converter '{typeAsString}' in Config");

        internal static Exception ConverterNotFound(Type converterType) => 
            new FieldConverterException($"Cannot find converter '{converterType}' in Config");

        private static string GetArgs(IEnumerable<Expression> arguments) => 
            string.IsNullOrEmpty(arguments.Skip(1).JoinToString(", ")) ? "source" : "source, " + arguments.Skip(1).JoinToString(", ");

    }

}