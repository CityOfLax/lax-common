using System;

namespace Lax.Helpers.EnumerationMapping {

    public struct EnumerationMapperCacheKey {
        
        public Type SourceType { get; set; }

        public Type DestinationType { get; set; }

    }

}