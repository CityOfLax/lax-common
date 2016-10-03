using System;

namespace Lax.Helpers.EnumerationMapping {

    [AttributeUsage(AttributeTargets.Field)]
    public class EnumerationMappingAttribute : Attribute {
        
        public Type DestinationType { get; set; }

        public object Value { get; set; }

        public EnumerationMappingAttribute(Type destinationType, object value) {
            DestinationType = destinationType;
            Value = value;
        }

    }

}