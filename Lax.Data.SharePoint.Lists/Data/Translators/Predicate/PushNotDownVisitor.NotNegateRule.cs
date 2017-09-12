using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Data.Translators.Predicate {

    internal partial class PushNotDownVisitor : ExpressionVisitor {
        private class NotNegateRule : INegateRule {

            public bool CanNegate(Expression node) {
                return node.NodeType == ExpressionType.Not;
            }

            public Expression Negate(Expression node) {
                var notNode = (UnaryExpression) node;

                return notNode.Operand;
            }

        }

    }

}