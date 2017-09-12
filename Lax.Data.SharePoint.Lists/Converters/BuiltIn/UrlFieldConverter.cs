using System;
using System.Collections.Generic;
using Lax.Data.SharePoint.Abstractions.Models;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("URL")]
    [UsedImplicitly]
    internal partial class UrlFieldConverter : MultiTypeFieldConverter {

        private static readonly IReadOnlyDictionary<Type, Func<IFieldConverter>> TypeConverters =
            new Dictionary<Type, Func<IFieldConverter>> {
                {typeof(string), () => new StringTypeConverter()},
                {typeof(UrlInfo), () => new UrlInfoTypeConverter()},
            };

        public override void Initialize(MetaField field) {
            base.Initialize(field);
            if (TypeConverters.ContainsKey(field.MemberType)) {
                Internal = TypeConverters[field.MemberType]();
            } else {
                throw new ArgumentException("MemberType is invalid");
            }
        }

    }

}