using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.DataTables {

    [HtmlTargetElement("datatable-empty")]
    public class DatatableEmptyTagHelper : TagHelper {

        [HtmlAttributeName("table-id")]
        public string TableId { get; set; } = "";

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "div";

            output.Attributes.SetAttribute("data-table-id", TableId);
            output.Attributes.SetAttribute("class", "datatable-empty hidden");

        }
    }

}