using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private interface ICombineRule {

            bool CanApplyAfterProjection(MethodCallExpression node);

            bool CanApplyAfterRowLimit(MethodCallExpression node);

            void Apply(RuleContext context, MethodCallExpression node);

            Expression Get(RuleContext context, MethodCallExpression node);

        }

    }

}