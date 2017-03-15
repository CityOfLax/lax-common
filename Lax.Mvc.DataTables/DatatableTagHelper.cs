using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.DataTables {

    [HtmlTargetElement("datatable")]
    public class DatatableTagHelper : TagHelper {

        [HtmlAttributeName("table-type")]
        public DataTableType TableType { get; set; } = DataTableType.Normal;

        [HtmlAttributeName("table-id")]
        public string TableId { get; set; } = "";

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "table";

            output.Attributes.SetAttribute("class", "table table-condensed table-responsive data-table");

            output.Attributes.SetAttribute("id", TableId);

            if (TableType == DataTableType.Basic) {
                output.Attributes.SetAttribute("data-type", "basic");
            } else if (TableType == DataTableType.Simple) {
                output.Attributes.SetAttribute("data-type", "simple");
            }

        }

    }

}