using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class AnyCombineRule : ICombineRule {

            public bool CanApplyAfterProjection(MethodCallExpression node) {
                return node.Arguments.Count == 1;
            }

            public bool CanApplyAfterRowLimit(MethodCallExpression node) {
                return node.Arguments.Count == 1;
            }

            public void Apply(RuleContext context, MethodCallExpression node) {
                if (node.Arguments.Count != 2) {
                    return;
                }

                var predicate = node.Arguments[1];
                context.ApplyFiltering(predicate);
            }

            public Expression Get(RuleContext context, MethodCallExpression node) {
                return SpQueryable.MakeAny(context.EntityType, context.ListItemsProvider, context.Query);
            }

        }

    }

}