using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.AdminLte {

    [HtmlTargetElement("lte-box-title")]
    public class AdminLteBoxTitleTagHelper : TagHelper {

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "h3";

            output.Attributes.SetAttribute("class", "box-title");

        }

    }

}