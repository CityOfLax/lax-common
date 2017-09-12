using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.Data.QueryModels;
using Lax.Data.SharePoint.Lists.Extensions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class InitContextRule : ICombineRule {

            public bool CanApplyAfterProjection(MethodCallExpression node) {
                return false;
            }

            public bool CanApplyAfterRowLimit(MethodCallExpression node) {
                return false;
            }

            public void Apply(RuleContext context, MethodCallExpression node) {
                context.Query = new QueryModel();
                context.ListItemsProvider =
                    (ISpListItemsProvider) ((ConstantExpression) node.Arguments[0].StripQuotes()).Value;

                var entityType = node.Method.GetGenericArguments()[0];
                context.ApplyContentType(entityType);
            }

            public Expression Get(RuleContext context, MethodCallExpression node) {
                var entityType = node.Method.GetGenericArguments()[0];
                return SpQueryable.MakeFetch(entityType, context.ListItemsProvider, context.Query);
            }

        }

    }

}