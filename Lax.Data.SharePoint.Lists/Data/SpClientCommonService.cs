using System.Collections.Generic;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.Configuration;
using Lax.Data.SharePoint.Lists.Converters;
using Lax.Data.SharePoint.Lists.Data.Mapper;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.MetaModels.Visitors;
using Lax.Data.SharePoint.Lists.Utils;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Data {

    public class SpClientCommonService : ICommonService {

        public SpClientCommonService([NotNull] ClientContext clientContext, [NotNull] Config config) {
            Guard.CheckNotNull(nameof(clientContext), clientContext);
            Guard.CheckNotNull(nameof(config), config);

            ClientContext = clientContext;
            Config = config;
        }

        public ClientContext ClientContext { get; }

        public Config Config { get; }

        public IReadOnlyCollection<IMetaModelVisitor> MetaModelProcessors => new List<IMetaModelVisitor> {
            new RuntimeInfoLoader(ClientContext),
            new FieldConverterCreator(Config.FieldConverters),
            new MapperInitializer()
        };

        public ISpListItemsProvider GetItemsProvider([NotNull] MetaList list) => new SpListItemsProvider(ClientContext, list);

    }

}