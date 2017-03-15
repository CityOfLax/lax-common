using Lax.Helpers.Booleans;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.AdminLte {

    [HtmlTargetElement("lte-box-header")]
    public class AdminLteBoxHeaderTagHelper : TagHelper {

        public bool Border { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "div";

            output.Attributes.SetAttribute("class", $"box-header{Border.ToStringIfTrue(" with-border")}");

        }

    }

}