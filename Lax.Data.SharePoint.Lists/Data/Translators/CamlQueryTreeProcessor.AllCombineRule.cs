using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class AllCombineRule : ICombineRule {

            public bool CanApplyAfterProjection(MethodCallExpression node) {
                return false;
            }

            public bool CanApplyAfterRowLimit(MethodCallExpression node) {
                return false;
            }

            public void Apply(RuleContext context, MethodCallExpression node) {
                var predicate = node.Arguments[1];
                context.ApplyFiltering(predicate, true);
            }

            public Expression Get(RuleContext context, MethodCallExpression node) {
                return SpQueryable.MakeNotAny(context.EntityType, context.ListItemsProvider, context.Query);
            }

        }

    }

}