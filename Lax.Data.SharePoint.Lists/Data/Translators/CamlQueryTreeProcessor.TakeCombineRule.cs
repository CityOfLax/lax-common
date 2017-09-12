using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.Extensions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class TakeCombineRule : ICombineRule {

            public bool CanApplyAfterProjection(MethodCallExpression node) {
                return true;
            }

            public bool CanApplyAfterRowLimit(MethodCallExpression node) {
                return false;
            }

            public void Apply(RuleContext context, MethodCallExpression node) {
                context.ApplyRowLimit((int) ((ConstantExpression) node.Arguments[1].StripQuotes()).Value);
            }

            public Expression Get(RuleContext context, MethodCallExpression node) {
                if (context.ProjectionApplied) {
                    return SpQueryable.MakeTake(context.EntityType, context.ProjectedType, context.ListItemsProvider,
                        context.Query, context.Projection);
                }

                return SpQueryable.MakeTake(context.EntityType, context.ListItemsProvider, context.Query);
            }

        }

    }

}