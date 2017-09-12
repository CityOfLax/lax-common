using System;
using System.Collections.Generic;
using System.Linq;
using Lax.Data.SharePoint.Abstractions.Models;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;
using Microsoft.SharePoint.Client.Taxonomy;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("Taxonomy")]
    [SpFieldConverter("TaxonomyMulti")]
    [UsedImplicitly]
    internal class TaxonomyFieldConverter : IFieldConverter {

        private bool _isMulti;
        private bool _isArray;

        private Guid? _termSetId;

        public void Initialize(MetaField field) {
            Guard.CheckNotNull("field", field);

            if (field.AllowMultipleValues) {
                if (field.MemberType != typeof(TermInfo[]) &&
                    !field.MemberType.IsAssignableFrom(typeof(List<TermInfo>))) {
                    
                    throw new ArgumentException("Only TermInfo[] or any class assigname from List<TermInfo> can be used as a member type.");
                }

                _isMulti = true;
                _isArray = field.MemberType.IsArray;
            } else {
                if (field.MemberType != typeof(TermInfo)) {
                    throw new ArgumentException(
                        "Only TermInfo can be used as a member type.");
                }
            }

            _termSetId = field.TermSetId;

        }

        public object FromSpValue(object value) {

            if (value == null) {
                return null;
            }

            if (!_isMulti) {
                return ConvertToTermInfo((TaxonomyFieldValue)value);
            }

            var fieldValues = (TaxonomyFieldValueCollection)value;
            var termValues = fieldValues.Select(ConvertToTermInfo).ToList();

            if (!termValues.Any()) {
                return new List<TermInfo>();
            }
            return _isArray ? (object)termValues.ToArray() : termValues;

        }

        public object ToSpValue(object value) {
            if (value == null) {
                return null;
            }

            if (!_isMulti) {
                var termValue = (TermInfo)value;

                return $"-1;#{termValue.Label}|{termValue.TermGuid}";
            }

            var termValues = ((IEnumerable<TermInfo>)value).Distinct().ToList();
            if (!termValues.Any()) {
                return null;
            }

            return string.Join(";", termValues.Select(tv => $"{tv.Label}|{tv.TermGuid}"));
        }

        public string ToCamlValue(object value) {
            if (value == null) {
                return null;
            }

            var singleValue = value as TermInfo;
            if (singleValue != null) {
                return singleValue.WssId.ToString();
            }

            //TODO: Fix this!!!!
            var multiValue = (IEnumerable<TermInfo>)value;
            return multiValue
                .Distinct()
                .Select(n => $"{n.WssId};#{n.Label}")
                .JoinToString(";#");
        }

        private TermInfo ConvertToTermInfo(TaxonomyFieldValue taxonomyFieldValue) =>
            new TermInfo {
                Label = taxonomyFieldValue.Label,
                TermGuid = taxonomyFieldValue.TermGuid,
                WssId = taxonomyFieldValue.WssId,
                TermSetId = _termSetId.ToString()
            };

    }

}