using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Extensions;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Lax.Data.SharePoint.Lists.Data.QueryModels {

    /// <summary>
    /// Represents CAML Query tag.
    /// </summary>
    [PublicAPI]
    public sealed partial class QueryModel {

        [CanBeNull] private List<FieldRefModel> _selectableFields;
        [CanBeNull] private List<OrderByModel> _orderBys;
        private bool _isOrderReversed;

        /// <summary>
        /// Gets or sets query row limit value.
        /// </summary>
        [CanBeNull]
        public int? RowLimit { get; set; }

        /// <summary>
        /// Gets inner operator for CAML Where tag.
        /// </summary>
        [CanBeNull]
        public WhereModel Where { get; private set; }

        /// <summary>
        /// Gets collection of specific selectable fields.
        /// </summary>
        [CanBeNull]
        public IEnumerable<FieldRefModel> SelectableFields =>
            _selectableFields?.Distinct(FieldRefModelComparer.Default);

        [CanBeNull]
        public IEnumerable<MemberRefModel> SelectableKnownFields {
            get {
                return _selectableFields?
                    .Where(n => n.Type == FieldRefType.KnownMember)
                    .Cast<MemberRefModel>()
                    .Distinct(MemberRefModelComparer.Default);
            }
        }

        /// <summary>
        /// Gets colelction of orderings for CAML OrderBy tag.
        /// </summary>
        [CanBeNull]
        public IEnumerable<OrderByModel> OrderBys {
            get {
                if (_isOrderReversed) {
                    return _orderBys.IsNullOrEmpty()
                        ? new[] {new OrderByModel(new KeyRefModel(), false)}
                        : _orderBys.Select(n => n.Reverse());
                }
                return _orderBys;
            }
        }

        /// <summary>
        /// Merge current CAML where operation with new one.
        /// </summary>
        /// <param name="where">New Where operation to merge.</param>
        public void MergeWheres([CanBeNull] WhereModel where) {
            Where = WhereModel.And(Where, @where);
        }

        /// <summary>
        /// Merge current CAML Orderby operations with new one.
        /// </summary>
        /// <param name="orderBy">New OrderBy operation to merge.</param>
        public void MergeOrderBys([CanBeNull] OrderByModel orderBy) {
            if (orderBy == null) {
                return;
            }

            if (_orderBys == null) {
                _orderBys = new List<OrderByModel>();
            }
            _orderBys.Add(_isOrderReversed ? orderBy.Reverse() : orderBy);
        }

        /// <summary>
        /// Merges current CAML selectable fields with new ones.
        /// </summary>
        /// <param name="selectableFields">New selectable fields to merge.</param>
        public void MergeSelectableFields([CanBeNull] IEnumerable<FieldRefModel> selectableFields) {
            if (selectableFields == null) {
                return;
            }

            if (_selectableFields == null) {
                _selectableFields = new List<FieldRefModel>();
            }
            _selectableFields.AddRange(selectableFields);
        }

        /// <summary>
        /// Replaces current CAML selectable fields with new ones.
        /// </summary>
        /// <param name="selectableFields">New selectable fields to replace.</param>
        public void ReplaceSelectableFields([CanBeNull] IEnumerable<FieldRefModel> selectableFields) {
            _selectableFields = selectableFields
                .EmptyIfNull()
                .ToList();
        }

        /// <summary>
        /// Reverses current ordering operations.
        /// </summary>
        public void ReverseOrder() {
            _isOrderReversed = !_isOrderReversed;
        }

        /// <summary>
        /// Resets ordering to default.
        /// </summary>
        public void ResetOrder() {
            _isOrderReversed = false;
            _orderBys = null;
        }

        /// <summary>
        /// Returns a <see cref="string"/> which represents the object instance.
        /// </summary>
        /// <returns>CAML-like string.</returns>
        /// <example>
        /// <![CDATA[<View>
        ///		<RowLimit></RowLimit>
        ///		<Query>
        ///			<Where></Where>
        ///			<OrderBy></OrderBy>
        ///		</Query>
        ///		<ViewFields></ViewFields>
        /// </View>]]>
        /// </example>
        public override string ToString() {
            var sb = new StringBuilder();
            using (new TagWriter(Tags.View, sb)) {
                TagWriter.Write(Tags.RowLimit, sb, RowLimit);

                using (new TagWriter(Tags.Query, sb)) {
                    TagWriter.Write(Tags.Where, sb, Where);
                    TagWriter.Write(Tags.OrderBy, sb, OrderBys);
                }

                TagWriter.Write(Tags.ViewFields, sb, SelectableFields);
            }
            return sb.ToString();
        }

    }

}