using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.DataTables {

    [HtmlTargetElement("datatable-search")]
    public class DatatableSearchTagHelper : TagHelper {

        public string TableId { get; set; } = "";

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "div";

            output.Content.SetHtmlContent($"<input type='text' class='form-control input-sm datatable-search-input' data-table-id='{TableId}' placeholder='Search...'>" +
                                          "<span class='glyphicon glyphicon-search form-control-feedback'></span>");

            output.Attributes.SetAttribute("class", "has-feedback pull-right");
            output.Attributes.SetAttribute("style", "display: inline; padding-left: 5px;");

        }

    }

}
