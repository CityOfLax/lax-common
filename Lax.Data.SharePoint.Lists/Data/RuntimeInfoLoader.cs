using System;
using Lax.Data.SharePoint.Lists.Extensions;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.MetaModels.Visitors;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Data {

    internal partial class RuntimeInfoLoader : BaseMetaModelVisitor {

        public RuntimeInfoLoader(ClientContext clientContext) {
            ClientContext = clientContext;
        }

        private ClientContext ClientContext { get; }

        public override void VisitContext(MetaContext context) {
            context.Url = ClientContext.Url;
            base.VisitContext(context);
        }

        public override void VisitList(MetaList list) {
            var spList = GetList(list);

            list.Title = spList.Title;
            list.IsExternal = spList.HasExternalDataSource;

            new ListInfoLoader(spList).VisitList(list);
        }

        private List GetList(MetaList list) {
            try {
                return ClientContext.GetListByUrl(list.Url, list.WebUrl);
            } catch (Exception e) {
                throw new ListNotFoundException(list, e);
            }
        }

    }

}