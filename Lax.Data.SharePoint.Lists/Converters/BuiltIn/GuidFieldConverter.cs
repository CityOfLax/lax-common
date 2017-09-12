using System;
using System.Collections.Generic;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("Guid")]
    [UsedImplicitly]
    internal partial class GuidFieldConverter : MultiTypeFieldConverter {

        private static readonly IReadOnlyDictionary<Type, Func<IFieldConverter>> TypeConverters =
            new Dictionary<Type, Func<IFieldConverter>> {
                {typeof(Guid), () => new GuidTypeConverter()},
                {typeof(Guid?), () => new NullableGuidTypeConverter()}
            };

        public override void Initialize(MetaField field) {
            base.Initialize(field);
            if (TypeConverters.ContainsKey(field.MemberType)) {
                Internal = TypeConverters[field.MemberType]();
            } else {
                throw new ArgumentException("Member type should be System.Guid or System.Nullable<System.Guid>.");
            }
        }

    }

}