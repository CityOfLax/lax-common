using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class LastRewriteRule : ICombineRule {

            public bool ThrowIfNothing { private get; set; }

            public bool CanApplyAfterProjection(MethodCallExpression node) {
                return node.Arguments.Count == 1;
            }

            public bool CanApplyAfterRowLimit(MethodCallExpression node) {
                return false;
            }

            public void Apply(RuleContext context, MethodCallExpression node) {
                context.Query.ReverseOrder();
                context.ApplyRowLimit(1);

                if (node.Arguments.Count != 2) {
                    return;
                }

                var predicate = node.Arguments[1];
                context.ApplyFiltering(predicate);
            }

            public Expression Get(RuleContext context, MethodCallExpression node) {
                if (context.ProjectionApplied) {
                    return SpQueryable.MakeFirst(context.EntityType, context.ProjectedType, context.ListItemsProvider,
                        context.Query,
                        ThrowIfNothing, false, context.Projection);
                }

                return SpQueryable.MakeFirst(context.EntityType, context.ListItemsProvider, context.Query,
                    ThrowIfNothing, false);
            }

        }

    }

}