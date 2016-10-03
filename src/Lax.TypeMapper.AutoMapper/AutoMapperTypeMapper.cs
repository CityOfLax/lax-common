namespace Lax.TypeMapper.AutoMapper {

    public class AutoMapperTypeMapper : ITypeMapper {

        public TDestination Map<TSource, TDestination>(TSource source) where TSource : class where TDestination : class {
            throw new System.NotImplementedException();
        }

    }

}