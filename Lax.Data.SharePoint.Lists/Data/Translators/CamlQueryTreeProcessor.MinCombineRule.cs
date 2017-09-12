using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class MinCombineRule : ICombineRule {

            public bool CanApplyAfterProjection(MethodCallExpression node) {
                return true;
            }

            public bool CanApplyAfterRowLimit(MethodCallExpression node) {
                return true;
            }

            public void Apply(RuleContext context, MethodCallExpression node) { }

            public Expression Get(RuleContext context, MethodCallExpression node) {
                if (context.Projection != null) {
                    return SpQueryable.MakeMin(context.EntityType, context.ProjectedType, context.ListItemsProvider,
                        context.Query, context.Projection);
                }

                return SpQueryable.MakeMin(context.EntityType, context.ListItemsProvider, context.Query);
            }

        }

    }

}