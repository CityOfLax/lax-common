using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.AdminLte {

    [HtmlTargetElement("lte-box-tools")]
    public class AdminLteBoxToolsTagHelper : TagHelper {

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "div";

            output.Attributes.SetAttribute("class", "box-tools pull-right");

        }

    }

}