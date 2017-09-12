using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.MetaModels.Providers;

namespace Lax.Data.SharePoint.Lists.Mappings.Annotation {

    internal class AnnotatedContextMapping<T> : IMetaContextProvider, IListUrlResolver {

        private readonly Type _contextType;
        private readonly List<AnnotatedListPart> _listProviders;

        public AnnotatedContextMapping() {
            _contextType = typeof(T);
            _listProviders = CreateListParts();
        }

        public string GetListUrlFromContextMember(MemberInfo member) {
            var listAttribute = member.GetCustomAttribute<SpListAttribute>(true);

            return listAttribute.Url;
        }

        public string GetWebUrlFromContextMember(MemberInfo member) {
            var listAttribute = member.GetCustomAttribute<SpListAttribute>(true);

            return listAttribute.WebUrl;
        }

        public Tuple<string, string> GetUrlsFromContextMember(MemberInfo member) => 
            new Tuple<string, string>(GetListUrlFromContextMember(member), GetWebUrlFromContextMember(member));

        public MetaContext GetMetaContext() => new MetaContext(_listProviders);

        #region [Private Methods]

        private List<AnnotatedListPart> CreateListParts() => 
            _contextType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(AnnotatedListPart.IsAnnotated)
                .GroupBy(GetUrlsFromContextMember)
                .Select(CreateListPart)
                .ToList();

        private static AnnotatedListPart CreateListPart(IGrouping<Tuple<string, string>, PropertyInfo> listProperties) => 
            AnnotatedListPart.Create(listProperties.Key.Item1, listProperties.Key.Item2, listProperties);

        #endregion

    }

}