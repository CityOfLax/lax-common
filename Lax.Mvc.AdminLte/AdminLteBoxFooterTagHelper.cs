using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.AdminLte {

    [HtmlTargetElement("lte-box-footer")]
    public class AdminLteBoxFooterTagHelper : TagHelper {

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "div";

            output.Attributes.SetAttribute("class", "box-footer");

        }

    }

}