using Lax.Helpers.Booleans;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.AdminLte {

    [HtmlTargetElement("lte-box-body")]
    public class AdminLteBoxBodyTagHelper : TagHelper {

        public bool Padding { get; set; } = true;

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "div";

            output.Attributes.SetAttribute("class", $"box-body{Padding.ToStringIfFalse(" no-padding")}");

        }

    }

}