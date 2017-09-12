using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.Extensions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class SelectCombineRule : ICombineRule {

            public bool CanApplyAfterProjection(MethodCallExpression node) {
                return false;
            }

            public bool CanApplyAfterRowLimit(MethodCallExpression node) {
                return true;
            }

            public void Apply(RuleContext context, MethodCallExpression node) {
                var predicate = node.Arguments[1];
                context.ApplyProjection(predicate);
            }

            public Expression Get(RuleContext context, MethodCallExpression node) {
                var lambdaNode = (LambdaExpression) node.Arguments[1].StripQuotes();

                return SpQueryable.MakeSelect(context.EntityType, context.ProjectedType, context.ListItemsProvider,
                    context.Query, lambdaNode);
            }

        }

    }

}