using System.Reflection;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.MetaModels.Providers;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Mappings.Annotation {

    internal class AnnotatedFieldPart : IMetaFieldProvider {

        private readonly MemberInfo _member;
        private readonly SpFieldAttribute _fieldAttribute;

        private AnnotatedFieldPart(MemberInfo member) {
            Guard.CheckNotNull(nameof(member), member);

            _member = member;
            _fieldAttribute = member.GetCustomAttribute<SpFieldAttribute>(true);
        }

        #region [Public Static]

        public static bool IsAnnotated(MemberInfo member) => 
            member.IsDefined(typeof(SpFieldAttribute)) && !member.IsDefined(typeof(SpFieldRemovedAttribute));

        public static AnnotatedFieldPart Create(PropertyInfo property) {
            Rules.CheckContentTypeField(property);

            return new AnnotatedFieldPart(property);
        }

        public static AnnotatedFieldPart Create(FieldInfo field) {
            Rules.CheckContentTypeField(field);

            return new AnnotatedFieldPart(field);
        }

        #endregion

        public MetaField GetMetaField(MetaContentType parent) => 
            new MetaField(parent, _member, string.IsNullOrEmpty(_fieldAttribute.Name)
                ? _member.Name
                : _fieldAttribute.Name) {
                CustomConverterType = _fieldAttribute.CustomConverterType,
                TypeAsString = _fieldAttribute.FieldType
            };

    }

}