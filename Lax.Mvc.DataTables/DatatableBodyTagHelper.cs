using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.DataTables {

    [HtmlTargetElement("datatable-body")]
    public class DatatableBodyTagHelper : TagHelper {

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "tbody";
        }

    }

}