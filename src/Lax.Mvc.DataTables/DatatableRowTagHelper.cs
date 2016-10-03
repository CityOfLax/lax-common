using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.DataTables {

    [HtmlTargetElement("datatable-row")]
    public class DatatableRowTagHelper : TagHelper {

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "tr";

        }

    }

}