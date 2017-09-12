using Lax.Data.SharePoint.Lists.Mappings.ClassLike;

namespace Lax.Data.SharePoint.Lists.Converters.Custom {

    public static class CustomFieldPartExtensions {

        public static FieldPart EnumFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("_Enum_");

        public static FieldPart JsonFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("_Json_");

        public static FieldPart KeyValueFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("_KeyValue_");

        public static FieldPart NumericRangeFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("_NumericRange_");

        public static FieldPart XmlFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("_Xml_");

    }

}