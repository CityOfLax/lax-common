﻿using System.Collections.Generic;
using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Data.Translators.Predicate {

    internal class SwapComparisonVisitor : ExpressionVisitor {

        private static readonly IReadOnlyDictionary<ExpressionType, ExpressionType> SwapMap = new Dictionary
            <ExpressionType, ExpressionType> {
                {ExpressionType.Equal, ExpressionType.Equal},
                {ExpressionType.NotEqual, ExpressionType.NotEqual},
                {ExpressionType.LessThanOrEqual, ExpressionType.GreaterThanOrEqual},
                {ExpressionType.LessThan, ExpressionType.GreaterThan},
                {ExpressionType.GreaterThan, ExpressionType.LessThan},
                {ExpressionType.GreaterThanOrEqual, ExpressionType.LessThanOrEqual}
            };

        protected override Expression VisitBinary(BinaryExpression node) {
            if (SwapMap.ContainsKey(node.NodeType) && node.Left.NodeType == ExpressionType.Constant) {
                return base.VisitBinary(Expression.MakeBinary(SwapMap[node.NodeType], node.Right, node.Left));
            }
            return base.VisitBinary(node);
        }

    }

}