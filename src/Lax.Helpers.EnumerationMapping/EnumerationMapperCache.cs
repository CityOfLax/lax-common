using System;
using System.Collections.Generic;
using System.Linq;
using Lax.Helpers.Reflection.Enumerations;

namespace Lax.Helpers.EnumerationMapping {

    internal class EnumerationMapperCache {

        private readonly Dictionary<EnumerationMapperCacheKey, Dictionary<object, object>> _cache = new Dictionary<EnumerationMapperCacheKey, Dictionary<object, object>>();

        public TDestination GetEnumerationMapping<TSource, TDestination>(TSource sourceValue) {

            var cacheKey = new EnumerationMapperCacheKey {
                SourceType = typeof(TSource),
                DestinationType = typeof(TDestination)
            };

            lock (this) {
                if (!_cache.ContainsKey(cacheKey)) {
                    _cache.Add(cacheKey, new Dictionary<object, object>());
                }
                if (!_cache[cacheKey].ContainsKey(sourceValue)) {
                    // Cache miss:
                    var destinationType = typeof(TDestination);

                    var sourceValueAttributes = (sourceValue as Enum).GetAttributesOfType<EnumerationMappingAttribute>();

                    if (sourceValueAttributes.All(sva => sva.DestinationType != destinationType)) {
                        throw new Exception("No Mapping found between Source and Destination");
                    }

                    var mappedValue = (TDestination)(sourceValueAttributes.First(sva => sva.DestinationType == destinationType).Value);

                    _cache[cacheKey].Add(sourceValue, mappedValue);

                }
            }

            return (TDestination) _cache[cacheKey][sourceValue];

        }

    }

}