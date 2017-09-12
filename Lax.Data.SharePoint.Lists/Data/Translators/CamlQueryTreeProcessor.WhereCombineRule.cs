using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class WhereCombineRule : ICombineRule {

            public bool CanApplyAfterProjection(MethodCallExpression node) {
                return false;
            }

            public bool CanApplyAfterRowLimit(MethodCallExpression node) {
                return false;
            }

            public void Apply(RuleContext context, MethodCallExpression node) {
                var predicate = node.Arguments[1];
                context.ApplyFiltering(predicate);
            }

            public Expression Get(RuleContext context, MethodCallExpression node) {
                return SpQueryable.MakeFetch(context.EntityType, context.ListItemsProvider, context.Query);
            }

        }

    }

}