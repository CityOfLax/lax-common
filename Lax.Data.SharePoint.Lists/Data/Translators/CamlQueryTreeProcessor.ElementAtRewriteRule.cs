using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.Extensions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class ElementAtRewriteRule : ICombineRule {

            public bool ThrowIfNothing { private get; set; }

            public Expression Get(RuleContext context, MethodCallExpression node) {
                var count = (int) ((ConstantExpression) node.Arguments[1].StripQuotes()).Value;

                if (context.ProjectionApplied) {
                    return SpQueryable.MakeElementAt(context.EntityType, context.ProjectedType,
                        context.ListItemsProvider, context.Query, count,
                        ThrowIfNothing, context.Projection);
                }

                return SpQueryable.MakeElementAt(context.EntityType, context.ListItemsProvider, context.Query, count,
                    ThrowIfNothing);
            }

            public bool CanApplyAfterProjection(MethodCallExpression node) {
                return true;
            }

            public bool CanApplyAfterRowLimit(MethodCallExpression node) {
                return false;
            }

            public void Apply(RuleContext context, MethodCallExpression node) { }

        }

    }

}