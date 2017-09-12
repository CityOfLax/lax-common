using System.Collections.Generic;
using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.CodeAnnotations;

namespace Lax.Data.SharePoint.Lists.Data.Translators.Predicate {

    internal partial class Evaluator : ExpressionVisitor {
        private class SubtreeEvaluator : ExpressionVisitor {

            [NotNull] private readonly HashSet<Expression> _candidates;

            internal SubtreeEvaluator([NotNull] HashSet<Expression> candidates) {
                _candidates = candidates;
            }

            public override Expression Visit(Expression exp) {
                if (exp == null) {
                    return null;
                }

                return _candidates.Contains(exp) ? Evaluate(exp) : base.Visit(exp);
            }

            private static Expression Evaluate(Expression e) {
                if (e.NodeType == ExpressionType.Constant) {
                    return e;
                }

                var func = Expression.Lambda(e).Compile();
                return Expression.Constant(func.DynamicInvoke(null), e.Type);
            }

        }

    }

}