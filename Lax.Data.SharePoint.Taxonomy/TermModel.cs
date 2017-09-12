using System.Collections.Generic;
using Lax.Data.SharePoint.Abstractions.Models;

namespace Lax.Data.SharePoint.Taxonomy {

    public class TermModel {

        public string Id { get; set; }

        public string Name { get; set; }

        public string PathOfTerm { get; set; }

        public List<TermModel> Terms { get; set; }

        public List<TermLabelModel> Labels { get; set; }

        public int Level { get; set; }

        public TermInfo ToTermInfo() => new TermInfo {
            TermGuid = Id,
            Label = Name,
            WssId = -1
        };

    }

}