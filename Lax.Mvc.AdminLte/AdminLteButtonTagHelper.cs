using Lax.Helpers.Booleans;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.AdminLte {

    [HtmlTargetElement("lte-btn")]
    public class AdminLteButtonTagHelper : AnchorTagHelper {

        [HtmlAttributeName("btn-type")]
        public string ButtonType { get; set; } = "default";

        [HtmlAttributeName("btn-size")]
        public string ButtonSize { get; set; } = "";

        [HtmlAttributeName("btn-flat")]
        public bool ButtonFlat { get; set; } = false;

        public AdminLteButtonTagHelper(IHtmlGenerator generator) : base(generator) {}

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            base.Process(context, output);

            output.TagName = "a";

            output.Attributes.SetAttribute("role", "button");
            output.Attributes.SetAttribute("class", $"btn btn-{ButtonType}{(!string.IsNullOrWhiteSpace(ButtonSize) ? $" btn-{ButtonSize}" : "")}{ButtonFlat.ToStringIfTrue(" btn-flat")}");

        }
    }

}