using System.Linq.Expressions;

namespace Lax.Data.SharePoint.Lists.Extensions {

    internal static class ExpressionExtensions {

        public static Expression StripQuotes(this Expression node) {
            while (node.NodeType == ExpressionType.Quote) {
                node = ((UnaryExpression) node).Operand;
            }
            return node;
        }

        public static bool IsConstant(this Expression node, object value) => 
            node.StripQuotes() is ConstantExpression && Equals(value, (node.StripQuotes() as ConstantExpression)?.Value);

    }

}