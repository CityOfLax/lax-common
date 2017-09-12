using System;
using System.Linq.Expressions;
using System.Reflection;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Configuration;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.Mappings.Annotation;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Data {

    /// <summary>
    /// Represents base data context class for CSOM.
    /// </summary>
    /// <typeparam name="TContext">Type of the data context.</typeparam>
    public abstract class SpClientContext<TContext> : SpContext<TContext>
        where TContext : SpClientContext<TContext> {

        /// <summary>
        /// Initializes a new instance of the <see cref="SpClientContext{TContext}"/> with specified <see cref="ClientContext"/> and <see cref="Config"/>.
        /// </summary>
        /// <param name="context">Client content to use for data access.</param>
        /// <param name="config">Configuration.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> or <paramref name="config"/> is null.</exception>
        protected SpClientContext([NotNull] ClientContext context, [NotNull] Config config)
            : base(new SpClientCommonService(context, config)) {
            Guard.CheckNotNull(nameof(context), context);

            ClientContext = context;
        }

        /// <summary>
        /// Gets <see cref="ClientContext"/> that is associated with the current data context.
        /// </summary>
        public ClientContext ClientContext { get; }

        /// <summary>
        /// Gets <see cref="List"/> instance by list accessor.
        /// </summary>
        /// <typeparam name="TEntity">Type of element.</typeparam>
        /// <param name="listSelector">List property accessor.</param>
        /// <returns>Instance of the <see cref="List"/>.</returns>
        public List GetSPList<TEntity>(Expression<Func<TContext, ISpList<TEntity>>> listSelector) => ClientContext.GetListByUrl(GetListUrl(listSelector));

        public Field GetFieldForProperty<TEntity>(
            Expression<Func<TContext, ISpList<TEntity>>> listSelector,
            Expression<Func<TEntity, object>> propertySelector) {

            var sharePointList = GetSPList(listSelector);

            var memberExpression = (MemberExpression)propertySelector.Body;
            var fieldAttribute = memberExpression.Member.GetCustomAttribute<SpFieldAttribute>(true);

            var internalFieldName = fieldAttribute.Name;

            var field = sharePointList.Fields.GetByInternalNameOrTitle(internalFieldName);

            ClientContext.Load(field);
            ClientContext.ExecuteQuery();

            return field;

        }

        public MetaField GetMetaFieldForProperty<TEntity>(
            Expression<Func<TContext, ISpList<TEntity>>> listSelector,
            Expression<Func<TEntity, object>> propertySelector) {

            var listUrl = GetListUrl(listSelector);

            var memberExpression = (MemberExpression)propertySelector.Body;

            return Model.Lists[listUrl].ContentTypes[typeof(TEntity)].Fields[memberExpression.Member.Name];

        }


    }

}