using System.Reflection;
using Lax.Data.SharePoint.Lists.Data;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Mappings.Annotation {

    internal sealed class AnnotatedMappingSource<TContext> : MappingSource<TContext>
        where TContext : ISpContext {

        private readonly AnnotatedContextMapping<TContext> _contextMapping;

        public AnnotatedMappingSource() {
            _contextMapping = new AnnotatedContextMapping<TContext>();
        }

        public override MetaContext GetMetaContext() => 
            _contextMapping.GetMetaContext();

        public override string GetListUrlFromContextMember(MemberInfo member) => 
            _contextMapping.GetListUrlFromContextMember(member);

    }

}