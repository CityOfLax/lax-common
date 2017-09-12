using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.CodeAnnotations;

namespace Lax.Data.SharePoint.Lists.Data.Translators.Predicate {

    internal partial class Evaluator : ExpressionVisitor {

        public Evaluator() {
            Nominator = new Nominator();
        }

        [NotNull]
        private Nominator Nominator { get; }

        public override Expression Visit(Expression node) {
            Nominator.Reset();
            Nominator.Visit(node);
            return new SubtreeEvaluator(Nominator.Candidates).Visit(node);
        }

    }

}