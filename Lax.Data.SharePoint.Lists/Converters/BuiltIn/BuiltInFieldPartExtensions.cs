using Lax.Data.SharePoint.Lists.Mappings.ClassLike;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    public static class BuiltInFieldPartExtensions {

        public static FieldPart BooleanFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Boolean");

        public static FieldPart DateTimeFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("DateTime");

        public static FieldPart GeoLocationFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Geolocation");

        public static FieldPart GuidFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Guid");

        public static FieldPart IntegerFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Integer");

        public static FieldPart CounterFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Counter");

        public static FieldPart LookupFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Lookup");

        public static FieldPart LookupMultiFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("LookupMulti");

        public static FieldPart MultiChoiceFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("MultiChoice");

        public static FieldPart NumberFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Number");

        public static FieldPart CurrencyFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Currency");

        public static FieldPart TaxonomyFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Taxonomy");

        public static FieldPart TaxonomyMultiFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("TaxonomyMulti");

        public static FieldPart TextFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Text");

        public static FieldPart NoteFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Note");

        public static FieldPart ChoiceFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("Choice");

        public static FieldPart UrlFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("URL");

        public static FieldPart UserFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("User");

        public static FieldPart UserMultiFieldType(this FieldPart fieldPart) => fieldPart.TypeAsString("UserMulti");

    }

}