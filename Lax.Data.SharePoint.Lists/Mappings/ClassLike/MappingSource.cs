using System.Reflection;
using Lax.Data.SharePoint.Lists.Data;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.MetaModels.Providers;

namespace Lax.Data.SharePoint.Lists.Mappings.ClassLike {

    internal class ClassLikeMappingSource<TContext> : MappingSource<TContext>
        where TContext : ISpContext {

        private readonly IListUrlResolver _listUrlResolver;
        private readonly IMetaContextProvider _metaContextProvider;

        public ClassLikeMappingSource(ContextMap<TContext> contextMap) {
            _listUrlResolver = contextMap;
            _metaContextProvider = contextMap;
        }

        public override MetaContext GetMetaContext() => 
            _metaContextProvider.GetMetaContext();

        public override string GetListUrlFromContextMember(MemberInfo member) => 
            _listUrlResolver.GetListUrlFromContextMember(member);

    }

}