using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class ReverseRewriteRule : ICombineRule {

            public Expression Get(RuleContext context, MethodCallExpression node) {
                return SpQueryable.MakeFetch(context.EntityType, context.ListItemsProvider, context.Query);
            }

            public bool CanApplyAfterProjection(MethodCallExpression node) {
                return true;
            }

            public bool CanApplyAfterRowLimit(MethodCallExpression node) {
                return false;
            }

            public void Apply(RuleContext context, MethodCallExpression node) {
                context.Query.ReverseOrder();
            }

        }

    }

}