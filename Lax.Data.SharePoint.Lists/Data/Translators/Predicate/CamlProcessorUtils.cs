using System.Linq.Expressions;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Data.QueryModels;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Data.Translators.Predicate {

    internal static class CamlProcessorUtils {

        [NotNull]
        internal static MemberRefModel GetFieldRef([NotNull] Expression node) {
            if (node.NodeType.In(new[] {ExpressionType.Convert, ExpressionType.ConvertChecked})) {
                node = ((UnaryExpression) node).Operand;
            }
            if (node.NodeType == ExpressionType.MemberAccess) {
                var memberNode = (MemberExpression) node;
                var objectNode = memberNode.Expression;
                if (objectNode != null &&
                    objectNode.NodeType.In(new[] {ExpressionType.Convert, ExpressionType.ConvertChecked})) {
                    var convertNode = (UnaryExpression) objectNode;
                    objectNode = convertNode.Operand;
                }
                if (objectNode != null && objectNode.NodeType == ExpressionType.Parameter) {
                    return new MemberRefModel(memberNode.Member);
                }
            }
            throw Error.SubqueryNotSupported(node);
        }

    }

}