using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Diagnostics;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {

        [NotNull] private static readonly IReadOnlyDictionary<MethodInfo, ICombineRule> CombineRules;

        static CamlQueryTreeProcessor() {
            CombineRules = new Dictionary<MethodInfo, ICombineRule>(new GenericMethodDefinitionComparer()) {
                {MethodUtils.SpqFakeFetch, new InitContextRule()},

                {MethodUtils.QSelect, new SelectCombineRule()},

                {MethodUtils.QMin, new MinCombineRule()},
                {MethodUtils.QMinP, new MinPCombineRule()},
                {MethodUtils.QMax, new MaxCombineRule()},
                {MethodUtils.QMaxP, new MaxPCombineRule()},

                {MethodUtils.QWhere, new WhereCombineRule()},
                {MethodUtils.QAny, new AnyCombineRule()},
                {MethodUtils.QAnyP, new AnyCombineRule()},
                {MethodUtils.QAll, new AllCombineRule()},

                {MethodUtils.QOrderBy, new OrderByCombineRule {ResetOrder = true, Ascending = true}},
                {MethodUtils.QOrderByDescending, new OrderByCombineRule {ResetOrder = true}},
                {MethodUtils.QThenBy, new OrderByCombineRule {Ascending = true}},
                {MethodUtils.QThenrByDescending, new OrderByCombineRule()},

                {MethodUtils.QTake, new TakeCombineRule()},

                {MethodUtils.QSingle, new FirstCombineRule {ThrowIfMultiple = true, ThrowIfNothing = true}},
                {MethodUtils.QSingleP, new FirstCombineRule {ThrowIfMultiple = true, ThrowIfNothing = true}},
                {MethodUtils.QSingleOrDefault, new FirstCombineRule {ThrowIfMultiple = true}},
                {MethodUtils.QSingleOrDefaultP, new FirstCombineRule {ThrowIfMultiple = true}},

                {MethodUtils.QFirst, new FirstCombineRule {ThrowIfNothing = true}},
                {MethodUtils.QFirstP, new FirstCombineRule {ThrowIfNothing = true}},
                {MethodUtils.QFirstOrDefault, new FirstCombineRule()},
                {MethodUtils.QFirstOrDefaultP, new FirstCombineRule()},

                {MethodUtils.QLast, new LastRewriteRule {ThrowIfNothing = true}},
                {MethodUtils.QLastP, new LastRewriteRule {ThrowIfNothing = true}},
                {MethodUtils.QLastOrDefault, new LastRewriteRule()},
                {MethodUtils.QLastOrDefaultP, new LastRewriteRule()},

                {MethodUtils.QElementAt, new ElementAtRewriteRule {ThrowIfNothing = true}},
                {MethodUtils.QElementAtOrDefault, new ElementAtRewriteRule()},

                {MethodUtils.QReverse, new ReverseRewriteRule()},

                {MethodUtils.QCount, new CountRewriteRule()},
                {MethodUtils.QCountP, new CountRewriteRule()}
            };
        }

        public Expression Process([NotNull] Expression node) {
            Logger.Debug(LogCategories.QueryTreeProcessor, "Original expressions tree:\n{0}", node);

            var result = new SubtreeCallsCombiner().Visit(node);

            Logger.Debug(LogCategories.QueryTreeProcessor, "Rewritten expressions tree:\n{0}", result);

            return result;
        }

    }

}