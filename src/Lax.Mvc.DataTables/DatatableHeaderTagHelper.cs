using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.DataTables {

    [HtmlTargetElement("datatable-header")]
    public class DatatableHeaderTagHelper : TagHelper {
        
        [HtmlAttributeName("orderable")]
        public bool Orderable { get; set; } = true;

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "th";

            output.Attributes.SetAttribute("data-orderable", Orderable ? "true" : "false");


        }

    }

}