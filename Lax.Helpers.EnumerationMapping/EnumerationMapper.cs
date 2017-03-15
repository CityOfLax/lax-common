namespace Lax.Helpers.EnumerationMapping {

    public static class EnumerationMapper {

        private static EnumerationMapperCache _cache = new EnumerationMapperCache();

        public static TDestination MapEnumerationValue<TSource, TDestination>(TSource sourceValue) =>
            _cache.GetEnumerationMapping<TSource, TDestination>(sourceValue);

    }

}