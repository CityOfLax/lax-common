using System;
using System.Reflection;
using Lax.Data.SharePoint.Lists.Extensions;

namespace Lax.Data.SharePoint.Lists.Utils.Reflection {

    internal static class ReflectionError {

        internal static Exception CtorNotFound(Type type, Type[] argumentTypes) =>
            new ArgumentException(
                $"Type '{type}' has no constructor that matches parameters list ({argumentTypes.JoinToString()})");

        internal static Exception CannotCreateGetter(MemberInfo member) =>
            new ArgumentException(
                $"Cannot create getter for member '{member.Name}' in type '{member.DeclaringType}'");

        internal static Exception CannotCreateSetter(MemberInfo member) =>
            new ArgumentException(
                $"Cannot create setter for member '{member.Name}' in type '{member.DeclaringType}'");

        internal static Exception CannotCreateGetterForIndexer(MemberInfo member) =>
            new ArgumentException(
                $"Cannot create getter for indexer '{member.Name}' in type '{member.DeclaringType}'");

        internal static Exception CannotCreateSetterForIndexer(MemberInfo member) =>
            new ArgumentException(
                $"Cannot create setter for indexer '{member.Name}' in type '{member.DeclaringType}'");

        internal static Exception CannotCreateGetterForWriteOnly(MemberInfo member) =>
            new ArgumentException(
                $"Cannot create getter for write-only member '{member.Name}' in type '{member.DeclaringType}'");

        internal static Exception CannotCreateSetterForReadOnly(MemberInfo member) =>
            new ArgumentException(
                $"Cannot create setter for read-only member '{member.Name}' in type '{member.DeclaringType}'");

    }

}