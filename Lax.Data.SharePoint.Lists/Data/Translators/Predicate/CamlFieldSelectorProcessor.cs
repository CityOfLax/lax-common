using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Data.QueryModels;
using Lax.Data.SharePoint.Lists.Diagnostics;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Data.Translators.Predicate {

    internal class CamlFieldSelectorProcessor : IProcessor<Expression, MemberRefModel> {

        [NotNull]
        public MemberRefModel Process([NotNull] Expression predicate) {
            Guard.CheckNotNull(nameof(predicate), predicate);

            Logger.Trace(LogCategories.FieldSelectorProcessor, "Original predicate:\n{0}", predicate);

            predicate = predicate.StripQuotes();
            if (predicate.NodeType == ExpressionType.Lambda) {
                predicate = ((LambdaExpression) predicate).Body;
            }

            var result = CamlProcessorUtils.GetFieldRef(predicate);

            Logger.Trace(LogCategories.FieldSelectorProcessor, "Selectable field in predicate:\n{0}", result);

            return result;
        }

    }

}