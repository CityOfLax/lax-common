using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.DataTables {

    [HtmlTargetElement("datatable-headers")]
    public class DatatableHeadersTagHelper : TagHelper {

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "tr";

            output.Attributes.SetAttribute("class", "datatable-header");
            output.PreElement.SetHtmlContent("<thead>");
            output.PostElement.SetHtmlContent("</thead>");

        }

    }

}