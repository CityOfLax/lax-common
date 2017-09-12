using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lax.Data.SharePoint.Abstractions.Models;
using Lax.Data.SharePoint.Lists.Data;

namespace Lax.Data.SharePoint.Taxonomy {

    public static class SpClientContextExtensions {

        public static async Task<TermSetModel> GetTermSetModelForFieldAsync<TContext, TEntity>(
            this SpClientContext<TContext> clientContext,
            ITaxonomyService taxonomyService,
            Expression<Func<TContext, ISpList<TEntity>>> listSelector,
            Expression<Func<TEntity, object>> propertySelector) where TContext : SpClientContext<TContext> {

            var metaField = clientContext.GetMetaFieldForProperty(listSelector, propertySelector);

            return await taxonomyService.GetTermSetAsync(clientContext.ClientContext, metaField.TermSetId.ToString());
        }

        public static async Task<TermModel> GetTermModelForTermInfoAsync<TContext>(
            this SpClientContext<TContext> clientContext,
            ITaxonomyService taxonomyService,
            TermInfo termInfo) where TContext : SpClientContext<TContext> => 
                termInfo != null ? await taxonomyService.GetTermAsync(
                    clientContext.ClientContext, 
                    termInfo.TermSetId,
                    termInfo.TermGuid) : null;

    }

}