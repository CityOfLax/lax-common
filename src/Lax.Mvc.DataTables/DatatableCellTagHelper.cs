using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.DataTables {

    [HtmlTargetElement("datatable-cell")]
    public class DatatableCellTagHelper : TagHelper {

        [HtmlAttributeName("search-value")]
        public string SearchValue { get; set; } = null;

        [HtmlAttributeName("order-value")]
        public string OrderValue { get; set; } = null;

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "td";

            if (SearchValue != null) {
                output.Attributes.SetAttribute("data-search", SearchValue);
            }

            if (OrderValue != null) {
                output.Attributes.SetAttribute("data-order", OrderValue);
            }

        }

    }

}