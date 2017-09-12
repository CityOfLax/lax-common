using System.Linq;
using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class SubtreeCallsCombiner : ExpressionVisitor {

            [NotNull] private readonly RuleContext _context = new RuleContext();

            public override Expression Visit(Expression node) {
                switch (node.NodeType) {
                    case ExpressionType.Call:
                        return VisitMethodCall((MethodCallExpression) node);
                    case ExpressionType.Quote:
                        return VisitUnary((UnaryExpression) node);
                }
                return node;
            }

            protected override Expression VisitMethodCall(MethodCallExpression node) {
                var rule = GetRuleAndUpdateContext(node);
                return rule.Get(_context, node);
            }

            private ICombineRule GetRuleAndUpdateContext(Expression node) {
                switch (node.NodeType) {
                    case ExpressionType.Call:
                        return GetRuleAndUpdateContext((MethodCallExpression) node);
                    case ExpressionType.Quote:
                        return GetRuleAndUpdateContext(((UnaryExpression) node).Operand);
                }
                throw Error.SubqueryNotSupported(node);
            }

            private ICombineRule GetRuleAndUpdateContext(MethodCallExpression node) {
                if (node.Method.DeclaringType == typeof(Queryable)) {
                    GetRuleAndUpdateContext(node.Arguments[0]);
                }

                if (!CombineRules.ContainsKey(node.Method)) {
                    throw Error.SubqueryNotSupported(node);
                }

                var currentRule = CombineRules[node.Method];

                ThrowIfCannotApplyAfterProjection(currentRule, node);
                ThrowIfCannotApplyAfterRowLimit(currentRule, node);

                currentRule.Apply(_context, node);

                return currentRule;
            }

            private void ThrowIfCannotApplyAfterProjection(ICombineRule rule, MethodCallExpression node) {
                if (!_context.ProjectionApplied) {
                    return;
                }

                if (rule.CanApplyAfterProjection(node)) {
                    return;
                }

                throw Error.SubqueryNotSupportedAfterProjection(node);
            }

            private void ThrowIfCannotApplyAfterRowLimit(ICombineRule rule, MethodCallExpression node) {
                if (!_context.RowLimitApplied) {
                    return;
                }

                if (rule.CanApplyAfterRowLimit(node)) {
                    return;
                }

                throw Error.SubqueryNotSupportedAfterRowLimit(node);
            }

        }

    }

}