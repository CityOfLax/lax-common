using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.Extensions;

namespace Lax.Data.SharePoint.Lists.Data.Translators.Predicate {

    internal partial class PushNotDownVisitor : ExpressionVisitor {

        public PushNotDownVisitor() {
            NegateRules = new List<INegateRule> {
                new ComparisonNegateRule(),
                new LogicalJoinNegateRule(),
                new NotNegateRule(),
                new BoolConstNegateRule()
            };
        }

        private IReadOnlyCollection<INegateRule> NegateRules { get; }

        protected override Expression VisitUnary(UnaryExpression node) {
            if (node.Method != null || node.NodeType != ExpressionType.Not) {
                return base.VisitUnary(node);
            }

            var operand = node.Operand.StripQuotes();

            var rule = NegateRules.FirstOrDefault(n => n.CanNegate(operand));
            return rule != null ? Visit(rule.Negate(operand)) : base.VisitUnary(node);
        }

        private class BoolConstNegateRule : INegateRule {

            public bool CanNegate(Expression node) {
                return node.NodeType == ExpressionType.Constant;
            }

            public Expression Negate(Expression node) {
                var constNode = (ConstantExpression) node;

                return constNode.Type == typeof(bool) ? Expression.Constant(!(bool) constNode.Value) : constNode;
            }

        }

    }

}