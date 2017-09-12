using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class OrderByCombineRule : ICombineRule {

            public bool Ascending { private get; set; }

            public bool ResetOrder { private get; set; }

            public bool CanApplyAfterProjection(MethodCallExpression node) {
                return false;
            }

            public bool CanApplyAfterRowLimit(MethodCallExpression node) {
                return false;
            }

            public void Apply(RuleContext context, MethodCallExpression node) {
                if (ResetOrder) {
                    context.Query.ResetOrder();
                }
                context.ApplyOrdering(node.Arguments[1], Ascending);
            }

            public Expression Get(RuleContext context, MethodCallExpression node) {
                return SpQueryable.MakeFetch(context.EntityType, context.ListItemsProvider, context.Query);
            }

        }

    }

}