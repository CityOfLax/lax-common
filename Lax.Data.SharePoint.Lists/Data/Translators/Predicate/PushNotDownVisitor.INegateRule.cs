using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Data.Translators.Predicate {

    internal partial class PushNotDownVisitor : ExpressionVisitor {

        private interface INegateRule {

            bool CanNegate(Expression node);

            Expression Negate(Expression node);

        }

    }

}