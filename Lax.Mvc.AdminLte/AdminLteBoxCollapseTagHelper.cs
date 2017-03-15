using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.AdminLte {

    [HtmlTargetElement("lte-box-collapse")]
    public class AdminLteBoxCollapseTagHelper : TagHelper {

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "button";

            output.Content.SetHtmlContent("<i class='fa fa-minus'></i>");

            output.Attributes.SetAttribute("class", "btn btn-box-tool");
            output.Attributes.SetAttribute("data-widget", "collapse");
            output.Attributes.SetAttribute("data-toggle", "tooltip");
            output.Attributes.SetAttribute("title", "Collapse");

            output.TagMode = TagMode.StartTagAndEndTag;

        }

    }

}