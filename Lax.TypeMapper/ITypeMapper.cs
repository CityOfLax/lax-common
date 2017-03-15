namespace Lax.TypeMapper {

    public interface ITypeMapper {

        TDestination Map<TSource, TDestination>(TSource source) where TSource : class where TDestination : class;

    }

}