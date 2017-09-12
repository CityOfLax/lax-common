using System.Collections.Generic;
using System.Threading.Tasks;
using Lax.Data.SharePoint.Abstractions.Models;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;

namespace Lax.Data.SharePoint.Taxonomy {

    public interface ITaxonomyService {

        //string AddTerm(ClientContext clientContext, TermQueryModel model);

        //string DeleteTerm(ClientContext clientContext, TermQueryModel model);

        TermSet GetParent(TermStore termStore, TermQueryModel model, out Term parentTerm);

        Task<TermModel> GetTermAsync(ClientContext clientContext, string termSetId, string termId);

        Task<TermSetModel> GetTermSetAsync(ClientContext clientContext, string termSetId);

        IEnumerable<TermInfo> GetTermSetTerms(ClientContext clientContext, string termSetId);

        IEnumerable<TermInfo> GetTermsForTaxonomyField(ClientContext clientContext, TaxonomyField taxonomyField);

    }

}