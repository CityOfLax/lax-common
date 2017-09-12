using System;
using System.Collections.Generic;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.MetaModels.Visitors;
using Untech.SharePoint.Common.CodeAnnotations;

namespace Lax.Data.SharePoint.Lists.Converters {

    internal sealed class FieldConverterFinder : BaseMetaModelVisitor {

        private FieldConverterFinder() {
            Converters = new List<Type>();
        }

        private List<Type> Converters { get; }

        [NotNull]
        public static IEnumerable<Type> Find([CanBeNull] IMetaModel model) {
            var finder = new FieldConverterFinder();

            finder.Visit(model);

            return finder.Converters;
        }

        public override void VisitField(MetaField field) {
            if (field.CustomConverterType != null) {
                Converters.Add(field.CustomConverterType);
            }
        }

    }

}