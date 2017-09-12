using System;
using System.Linq;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.MetaModels.Visitors;
using Lax.Data.SharePoint.Lists.Utils;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;

namespace Lax.Data.SharePoint.Lists.Data {

    internal partial class RuntimeInfoLoader : BaseMetaModelVisitor {

        private class ListInfoLoader : BaseMetaModelVisitor {

            public ListInfoLoader(List spList) {
                Guard.CheckNotNull(nameof(spList), spList);

                SpList = spList;
            }

            private List SpList { get; }

            public override void VisitContentType(MetaContentType contentType) {
                var spContentType = SpList.ContentTypes
                    .Where(n => n.StringId.StartsWith(contentType.Id ?? "0x01"))
                    .OrderBy(n => n.StringId.Length)
                    .FirstOrDefault();

                if (spContentType == null) {
                    throw new ContentTypeNotFoundException(contentType);
                }

                contentType.Id = spContentType.Id.ToString();
                contentType.Name = spContentType.Name;

                base.VisitContentType(contentType);
            }

            public override void VisitField(MetaField field) {
                var spField = GetField(field);

                field.Id = spField.Id;
                field.Title = spField.Title;
                field.ReadOnly = spField.ReadOnlyField;
                field.Required = spField.Required;

                field.IsCalculated = spField.FieldTypeKind == FieldType.Computed ||
                                     spField.FieldTypeKind == FieldType.Calculated;
                if (string.IsNullOrEmpty(field.TypeAsString)) {
                    field.TypeAsString = spField.TypeAsString;
                }

                if (spField.FieldTypeKind == FieldType.Lookup) {
                    var spLookupField = SpList.Context.CastTo<FieldLookup>(spField);

                    field.AllowMultipleValues = spLookupField.AllowMultipleValues;
                    field.LookupList = spLookupField.LookupList;
                    field.LookupField = spLookupField.LookupField;
                }
                if (spField.FieldTypeKind == FieldType.User) {
                    var spUserField = SpList.Context.CastTo<FieldUser>(spField);

                    field.AllowMultipleValues = spUserField.AllowMultipleValues;
                }
                if (spField.FieldTypeKind == FieldType.MultiChoice) {
                    field.AllowMultipleValues = true;
                }
                if (spField.FieldTypeKind == FieldType.Calculated) {
                    var spCalculatedField = SpList.Context.CastTo<FieldCalculated>(spField);
                    field.OutputType = spCalculatedField.OutputType.ToString();
                }
                if (spField.TypeAsString.Equals("TaxonomyFieldTypeMulti") ||
                    spField.TypeAsString.Equals("TaxonomyFieldType")) {

                    field.TermSetId = (spField.TypedObject as TaxonomyField).TermSetId;

                }
                if (spField.TypeAsString.Equals("TaxonomyFieldTypeMulti")) {
                    field.AllowMultipleValues = true;
                }
            }

            private Field GetField(MetaField field) {
                try {
                    var spField = SpList.Fields.GetByInternalNameOrTitle(field.InternalName);

                    SpList.Context.Load(spField);
                    SpList.Context.ExecuteQuery();

                    return spField;
                } catch (Exception e) {
                    throw new FieldNotFoundException(field, e);
                }
            }

        }

    }

}