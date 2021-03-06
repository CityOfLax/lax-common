using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Data;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.Mappings.Annotation;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.MetaModels.Providers;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Mappings.ClassLike {

    /// <summary>
    /// Represents provider of <see cref="MetaList"/> that allows to configure list mapping in fluent way.
    /// </summary>
    public sealed class ListPart<TContext> : IMetaListProvider
        where TContext : ISpContext {

        private readonly ContextMap<TContext> _contextMap;
        private readonly Dictionary<Type, IMetaContentTypeProvider> _contentTypes;

        internal ListPart(ContextMap<TContext> contextMap, string listUrl, string webUrl) {
            _contextMap = contextMap;
            ListUrl = listUrl;
            WebUrl = webUrl;

            _contentTypes = new Dictionary<Type, IMetaContentTypeProvider>();
        }

        internal string ListUrl { get; }

        internal string WebUrl { get; }

        /// <summary>
        /// Registers content type mapping.
        /// </summary>
        /// <param name="propertyAccessor">Field or property accessor.</param>
        /// <param name="contentTypeMap">The instance of the <see cref="ContentTypeMap{TEntity}"/> 
        ///		that will be associated with <typeparamref name="TEntity"/> type and with specified member of data context.</param>
        /// <returns>Current instance.</returns>
        /// <exception cref="ArgumentException">Invalid member was selected or mapping for type <typeparamref name="TEntity"/> was added earlier to that or another list.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyAccessor"/> or <paramref name="contentTypeMap"/> is null.</exception>
        [NotNull]
        public ListPart<TContext> ContentType<TEntity>(
            [NotNull] Expression<Func<TContext, ISpList<TEntity>>> propertyAccessor,
            [NotNull] ContentTypeMap<TEntity> contentTypeMap) {
            Guard.CheckNotNull(nameof(propertyAccessor), propertyAccessor);
            Guard.CheckNotNull(nameof(contentTypeMap), contentTypeMap);

            RegisterContentTypeMap(propertyAccessor, contentTypeMap);

            return this;
        }

        /// <summary>
        /// Registers content type mapping based on <see cref="Mappings.Annotation"/>.
        /// </summary>
        /// <param name="propertyAccessor">Field or property accessor.</param>
        /// <returns>Current instance.</returns>
        /// <exception cref="ArgumentException">Invalid member was selected or mapping for type <typeparamref name="TEntity"/> was added earlier to that or another list.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyAccessor"/> is null.</exception>
        [NotNull]
        public ListPart<TContext> AnnotatedContentType<TEntity>(
            [NotNull] Expression<Func<TContext, ISpList<TEntity>>> propertyAccessor) {
            Guard.CheckNotNull(nameof(propertyAccessor), propertyAccessor);

            RegisterContentTypeMap(propertyAccessor, new AnnotatedContentTypeMapping(typeof(TEntity)));

            return this;
        }

        MetaList IMetaListProvider.GetMetaList(MetaContext parent) {
            return new MetaList(parent, ListUrl, _contentTypes.Values.ToList(), WebUrl);
        }

        private void RegisterContentTypeMap<TEntity>(Expression<Func<TContext, ISpList<TEntity>>> propertyAccessor,
            IMetaContentTypeProvider contentTypeMap) {
            var member = GetMemberInfo(propertyAccessor);

            CheckMember(member);

            if (_contextMap.MemberToListMap.ContainsKey(member)) {
                throw new ArgumentException($"Property {member} was added earlier to that or another list.");
            }
            _contextMap.MemberToListMap.Add(member, this);

            var entityType = typeof(TEntity);
            if (_contentTypes.ContainsKey(entityType)) {
                throw new ArgumentException(
                    $"ContentTypeMap was already registered for type {entityType} in that list.");
            }
            _contentTypes.Add(typeof(TEntity), contentTypeMap);
        }

        private void CheckMember(MemberInfo member) {
            if (member.MemberType == MemberTypes.Property) {
                Rules.CheckContextList((PropertyInfo) member);
            } else {
                throw new ArgumentException($"Invalid member info {member}");
            }
        }

        private MemberInfo GetMemberInfo(LambdaExpression lambda) {
            var body = lambda.Body.StripQuotes();
            if (body.NodeType.In(new[] {ExpressionType.Convert, ExpressionType.ConvertChecked})) {
                body = ((UnaryExpression) body).Operand;
            }
            if (body.NodeType == ExpressionType.MemberAccess) {
                return ((MemberExpression) body).Member;
            }
            throw new ArgumentException($"{lambda} is not a valid field or property accessor.");
        }

    }

}