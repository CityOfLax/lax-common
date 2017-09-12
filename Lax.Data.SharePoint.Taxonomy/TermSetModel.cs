using System.Collections.Generic;

namespace Lax.Data.SharePoint.Taxonomy {

    public class TermSetModel {

        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsOpenForTermCreation { get; set; }

        public string CustomSortOrder { get; set; }

        public List<TermModel> Terms { get; set; }

        public List<TermModel> FlatTerms { get; set; }
    }

}