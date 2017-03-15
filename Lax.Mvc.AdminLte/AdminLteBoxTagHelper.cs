using Lax.Helpers.Booleans;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.AdminLte {

    [HtmlTargetElement("lte-box")]
    public class AdminLteBoxTagHelper : TagHelper {

        public bool Solid { get; set; } = false;

        public bool Collapsed { get; set; } = false;

        public string BoxType { get; set; } = "default";

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = "div";

            output.Attributes.SetAttribute("class", $"box box-{BoxType}{Solid.ToStringIfTrue(" box-solid")}{Collapsed.ToStringIfTrue(" collapsed-box")}");

        }

    }

}