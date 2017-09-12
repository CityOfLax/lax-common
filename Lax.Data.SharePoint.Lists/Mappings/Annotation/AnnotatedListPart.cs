using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.MetaModels.Providers;
using Lax.Data.SharePoint.Lists.Utils;

namespace Lax.Data.SharePoint.Lists.Mappings.Annotation {

    internal class AnnotatedListPart : IMetaListProvider {

        private readonly string _url;
        private readonly string _webUrl;
        private readonly Dictionary<Type, AnnotatedContentTypeMapping> _contentTypeProviders;

        private AnnotatedListPart(string listUrl, string webUrl) {
            Guard.CheckNotNullOrEmpty(nameof(listUrl), listUrl);

            _url = listUrl;
            _webUrl = webUrl;
            _contentTypeProviders = new Dictionary<Type, AnnotatedContentTypeMapping>();
        }

        #region [Public Static]

        public static bool IsAnnotated(PropertyInfo property) => 
            property.IsDefined(typeof(SpListAttribute));

        public static AnnotatedListPart Create(string listUrl, string webUrl, IEnumerable<PropertyInfo> contextProperties) {
            var listMapping = new AnnotatedListPart(listUrl, webUrl);

            contextProperties.Each(listMapping.RegisterContentType);

            return listMapping;
        }

        #endregion


        public MetaList GetMetaList(MetaContext parent) => 
            new MetaList(parent, _url, _contentTypeProviders.Values.ToList(), _webUrl);

        #region [Private Methods]

        private void RegisterContentType(PropertyInfo contextProperty) {
            Rules.CheckContextList(contextProperty);

            var entityType = contextProperty.PropertyType.GetGenericArguments()[0];

            RegisterContentType(entityType);
        }


        private void RegisterContentType(Type entityType) {
            if (!_contentTypeProviders.ContainsKey(entityType)) {
                _contentTypeProviders.Add(entityType, new AnnotatedContentTypeMapping(entityType));
            }
        }

        #endregion

    }

}