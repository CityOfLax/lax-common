using System;
using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.Data.QueryModels;
using Lax.Data.SharePoint.Lists.Data.Translators.Predicate;
using Lax.Data.SharePoint.Lists.Extensions;

namespace Lax.Data.SharePoint.Lists.Data.Translators {

    internal sealed partial class CamlQueryTreeProcessor : IProcessor<Expression, Expression> {
        private class RuleContext {

            public QueryModel Query { get; set; }

            public ISpListItemsProvider ListItemsProvider { get; set; }

            public Type EntityType { get; private set; }

            public LambdaExpression Projection { get; private set; }

            public Type ProjectedType { get; private set; }

            public bool ProjectionApplied { get; private set; }

            public bool RowLimitApplied { get; private set; }

            public void ApplyContentType(Type entityType) {
                EntityType = entityType;
            }

            public void ApplyFiltering(Expression predicate, bool negate = false) {
                var where = new CamlPredicateProcessor().Process(predicate);
                if (negate) {
                    where = where.Negate();
                }
                Query.MergeWheres(where);
            }

            public void ApplyOrdering(Expression predicate, bool ascending) {
                Query.MergeOrderBys(new OrderByModel(new CamlFieldSelectorProcessor().Process(predicate), ascending));
            }

            public void ApplyProjection(Expression predicate) {
                Query.MergeSelectableFields(new CamlSelectableFieldsProcessor().Process(predicate));

                Projection = (LambdaExpression) predicate.StripQuotes();
                ProjectedType = Projection.ReturnType;
                ProjectionApplied = true;
            }

            public void ApplyRowLimit(int rowLimit) {
                Query.RowLimit = rowLimit;
                RowLimitApplied = true;
            }

        }

    }

}