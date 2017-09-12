using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundatio.Caching;
using Lax.Data.SharePoint.Abstractions.Models;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;

namespace Lax.Data.SharePoint.Taxonomy {

    public class TaxonomyService : ITaxonomyService {

        private readonly ICacheClient _cacheClient;

        private static readonly TimeSpan _cacheExpirationTimeSpan = new TimeSpan(0, 10, 0); //Expires in 10mins

        public TaxonomyService(
            ICacheClient cacheClient) {

            _cacheClient = cacheClient;
        }

        //public string AddTerm(ClientContext clientContext, TermQueryModel model) {

        //    var pickerTerm = new TermModel();

        //    if (clientContext != null) {
        //        var taxonomySession = TaxonomySession.GetTaxonomySession(clientContext);
        //        var termStore = taxonomySession.GetDefaultSiteCollectionTermStore();

        //        var termSet = GetParent(termStore, model, out Term parentTerm);
        //        Term newTerm = null;
        //        var newTermId = new Guid(model.Id);
        //        if (parentTerm != null) {
        //            clientContext.Load(parentTerm, t => t.PathOfTerm,
        //                                       t => t.Id,
        //                                       t => t.Labels,
        //                                       t => t.Name,
        //                                       t => t.Terms);
        //            clientContext.ExecuteQuery();
        //            newTerm = parentTerm.CreateTerm(model.Name, model.LCID, newTermId);
        //        } else {
        //            clientContext.Load(termSet);
        //            clientContext.ExecuteQuery();
        //            newTerm = termSet.CreateTerm(model.Name, 1033, newTermId);
        //        }

        //        clientContext.Load(newTerm, t => t.PathOfTerm,
        //                                        t => t.Id,
        //                                        t => t.Labels,
        //                                        t => t.Name);
        //        clientContext.ExecuteQuery();

        //        pickerTerm.Name = newTerm.Name;
        //        pickerTerm.Id = newTerm.Id.ToString();
        //        pickerTerm.PathOfTerm = newTerm.PathOfTerm;
        //        pickerTerm.Level = newTerm.PathOfTerm.Split(';').Length - 1;
        //        pickerTerm.Terms = new List<TermModel>();
        //    }
        //    return JsonHelper.Serialize<TermModel>(pickerTerm);
        //}

        public TermSet GetParent(TermStore termStore, TermQueryModel model, out Term parentTerm) {
            TermSet termSet = null;
            parentTerm = null;

            if (string.IsNullOrEmpty(model.TermSetId)) {
                parentTerm = termStore.GetTerm(new Guid(model.ParentTermId));
            } else {
                termSet = termStore.GetTermSet(new Guid(model.TermSetId));
            }

            return termSet;
        }

        public async Task<TermModel> GetTermAsync(ClientContext clientContext, string termSetId, string termId) {

            var termCacheKey = TermCacheKey(termSetId, termId);

            if (!await _cacheClient.ExistsAsync(termCacheKey)) {

                var termSetModel = await GetTermSetAsync(clientContext, termSetId);

                return termSetModel.FlatTerms.FirstOrDefault(t => t.Id.Equals(termId));

            }

            return (await _cacheClient.GetAsync<TermModel>(termCacheKey)).Value;

        }


        private async Task<TermModel> GetTermAsync(Term term, string termSetId, bool includeChildren = true) {
            
            var pTerm = new TermModel() {
                Name = term.Name,
                Id = Convert.ToString(term.Id),
                PathOfTerm = term.PathOfTerm,
                Level = term.PathOfTerm.Split(';').Length - 1,
                Terms = new List<TermModel>(),
                Labels = new List<TermLabelModel>()
            };

            term.Labels.ToList<Label>().ForEach(l => pTerm.Labels.Add(new TermLabelModel() {
                Value = l.Value,
                IsDefaultForLanguage = l.IsDefaultForLanguage
            }));


            if (term.TermsCount > 0 && includeChildren == true) {
                term.Context.Load(term.Terms, terms => terms.Include(t => t.PathOfTerm,
                                                    t => t.Id,
                                                    t => t.Labels.Include(l => l.IsDefaultForLanguage, l => l.Value),
                                                    t => t.Name,
                                                    t => t.TermsCount));
                term.Context.ExecuteQuery();
                term.Terms.ToList<Term>().ForEach(async t => pTerm.Terms.Add(await GetTermAsync(t, termSetId)));
            }

            if (!includeChildren) {
                await _cacheClient.SetAsync(
                    TermCacheKey(term.Id.ToString(), termSetId), 
                    pTerm,
                    _cacheExpirationTimeSpan);
            }

            return pTerm;
        }

        public async Task<TermSetModel> GetTermSetAsync(ClientContext clientContext, string termSetId) {

            var termSetCacheKey = TermSetCacheKey(termSetId);

            if (await _cacheClient.ExistsAsync(termSetCacheKey)) {
                return (await _cacheClient.GetAsync<TermSetModel>(termSetCacheKey)).Value;
            }

            var pickerTermSet = new TermSetModel();

            if (clientContext != null) {
                //Get terms from the 'Keywords' termset for autocomplete suggestions.
                // It might be a good idea to cache these values.

                var taxonomySession = TaxonomySession.GetTaxonomySession(clientContext);
                var termStore = taxonomySession.GetDefaultSiteCollectionTermStore();

                TermSet termSet = termStore.GetTermSet(new Guid(termSetId));

                clientContext.Load(termSet, ts => ts.Id, ts => ts.IsOpenForTermCreation, ts => ts.CustomSortOrder, ts => ts.Name,
                    ts => ts.Terms.Include(t => t.PathOfTerm,
                                            t => t.Id,
                                            t => t.Labels.Include(l => l.IsDefaultForLanguage, l => l.Value),
                                            t => t.Name,
                                            t => t.TermsCount));
                clientContext.ExecuteQuery();

                var allTerms = termSet.GetAllTerms();

                clientContext.Load(allTerms, terms => terms.Include(t => t.PathOfTerm,
                                                    t => t.Id,
                                                    t => t.Labels.Include(l => l.IsDefaultForLanguage, l => l.Value),
                                                    t => t.Name,
                                                    t => t.TermsCount));
                clientContext.ExecuteQuery();

                pickerTermSet.Id = termSet.Id.ToString().Replace("{", string.Empty).Replace("}", string.Empty);
                pickerTermSet.Name = termSet.Name;
                pickerTermSet.IsOpenForTermCreation = termSet.IsOpenForTermCreation;
                pickerTermSet.CustomSortOrder = termSet.CustomSortOrder;
                pickerTermSet.Terms = new List<TermModel>();
                pickerTermSet.FlatTerms = new List<TermModel>();

                foreach (var term in termSet.Terms.ToList<Term>()) {
                    pickerTermSet.Terms.Add(await GetTermAsync(term, termSetId));
                }

                foreach (var term in allTerms.ToList<Term>()) {
                    pickerTermSet.FlatTerms.Add(await GetTermAsync(term, termSetId, false));
                }

            }

            await _cacheClient.SetAsync(termSetCacheKey, pickerTermSet, _cacheExpirationTimeSpan);

            return pickerTermSet;
        }

        //public string DeleteTerm(ClientContext clientContext, TermQueryModel model) {
        //    var pickerTermSet = new TermSetModel();
        //    //var searchString = (string)HttpContext.Current.Request["SearchString"];

        //    if (clientContext != null) {
        //        //Get terms from the 'Keywords' termset for autocomplete suggestions.
        //        // It might be a good idea to cache these values.

        //        var taxonomySession = TaxonomySession.GetTaxonomySession(clientContext);
        //        var termStore = taxonomySession.GetDefaultKeywordsTermStore();
        //        var termToDelete = termStore.GetTerm(new Guid(model.Id));

        //        clientContext.Load(termToDelete);
        //        clientContext.ExecuteQuery();

        //        termToDelete.DeleteObject();
        //        clientContext.ExecuteQuery();

        //        var termSetId = new Guid(model.TermSetId);
        //        var termSet = termStore.GetTermSet(termSetId);

        //        clientContext.Load(termSet, ts => ts.Id, ts => ts.IsOpenForTermCreation, ts => ts.CustomSortOrder, ts => ts.Name,
        //          ts => ts.Terms.Include(t => t.PathOfTerm,
        //                                    t => t.Id,
        //                                    t => t.Labels.Include(l => l.IsDefaultForLanguage, l => l.Value),
        //                                    t => t.Name,
        //                                    t => t.TermsCount));
        //        clientContext.ExecuteQuery();

        //        pickerTermSet.Id = termSet.Id.ToString().Replace("{", string.Empty).Replace("}", string.Empty);
        //        pickerTermSet.Name = termSet.Name;
        //        pickerTermSet.IsOpenForTermCreation = termSet.IsOpenForTermCreation;
        //        pickerTermSet.CustomSortOrder = termSet.CustomSortOrder;
        //        pickerTermSet.Terms = new List<TermModel>();

        //        foreach (var term in termSet.Terms.ToList<Term>()) {
        //            pickerTermSet.Terms.Add(GetTermAsync(term));
        //        }
        //    }
        //    return JsonHelper.Serialize<TermSetModel>(pickerTermSet);
        //}

        public IEnumerable<TermInfo> GetTermsForTaxonomyField(
            ClientContext clientContext,
            TaxonomyField taxonomyField) =>
                GetTermSetTerms(clientContext, taxonomyField.TermSetId.ToString());

        public IEnumerable<TermInfo> GetTermSetTerms(ClientContext clientContext, string termSetId) {

            var taxonomySession = TaxonomySession.GetTaxonomySession(clientContext);
            var termStore = taxonomySession.GetDefaultSiteCollectionTermStore();

            var termSet = termStore.GetTermSet(new Guid(termSetId));

            clientContext.Load(termSet, ts => ts.Id, ts => ts.IsOpenForTermCreation, ts => ts.CustomSortOrder, ts => ts.Name,
                ts => ts.Terms.Include(t => t.PathOfTerm,
                                        t => t.Id,
                                        t => t.Labels.Include(l => l.IsDefaultForLanguage, l => l.Value),
                                        t => t.Name,
                                        t => t.TermsCount));
            clientContext.ExecuteQuery();

            var allTerms = termSet.GetAllTerms();

            clientContext.Load(allTerms, terms => terms.Include(t => t.PathOfTerm,
                                                t => t.Id,
                                                t => t.Labels.Include(l => l.IsDefaultForLanguage, l => l.Value),
                                                t => t.Name,
                                                t => t.TermsCount));
            clientContext.ExecuteQuery();

            var wssIds = GetWssIdsForList(clientContext);

            return allTerms.Select(t => new TermInfo {
                Label = t.Labels.FirstOrDefault().Value,
                TermGuid = t.Id.ToString(),
                WssId = wssIds.ContainsKey(t.Id.ToString()) ? wssIds[t.Id.ToString()] : 0
            }).AsEnumerable();

        }

        private int GetWssIdByTermId(ClientContext clientContext, string termGuidString) {
            var result = 0;
            var web = clientContext.Site.RootWeb;
            var list = web.Lists.GetByTitle("TaxonomyHiddenList");
            var q = new CamlQuery() {
                ViewXml =
                    $@"
                 <View>
                    <Query>
                       <Where>
                          <Eq>
                             <FieldRef Name='IdForTerm' />
                             <Value Type='Text'>{termGuidString}</Value>
                          </Eq>
                       </Where>
                    </Query>
                 </View>"
            };
            var items = list.GetItems(q);
            clientContext.Load(items);
            clientContext.ExecuteQuery();
            if (items.Count == 1) {
                result = int.Parse(items[0]["ID"].ToString());
            }

            return result;
        }

        private IDictionary<string, int> GetWssIdsForList(ClientContext clientContext) {
            var result = 0;
            var web = clientContext.Site.RootWeb;
            var list = web.Lists.GetByTitle("TaxonomyHiddenList");
            var q = new CamlQuery() {
                ViewXml =
                    $@"
                 <View>
                    <Query>
                    </Query>
                 </View>"
            };
            var items = list.GetItems(q);
            clientContext.Load(items);
            clientContext.ExecuteQuery();

            return items.ToDictionary(i => i["IdForTerm"].ToString(), i => int.Parse(i["ID"].ToString()));
        }

        private string TermSetCacheKey(string termSetId) =>
            $"{GetType().FullName}_TermSet_{termSetId}";

        private string TermCacheKey(string termSetId, string termId) =>
            $"{GetType().FullName}_Term_{termSetId}_{termId}";

    }

}