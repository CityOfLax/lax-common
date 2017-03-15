using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.Bootstrap {

    [HtmlTargetElement("bs-col")]
    public class BootstrapColumnTagHelper : TagHelper {
        
        [HtmlAttributeName("lg")]
        public int? LargeSpan { get; set; }

        [HtmlAttributeName("md")]
        public int? MediumSpan { get; set; }

        [HtmlAttributeName("sm")]
        public int? SmallSpan { get; set; }

        [HtmlAttributeName("xs")]
        public int? ExtraSmallSpan { get; set; }

        [HtmlAttributeName("all")]
        public int? AllSpan { get; set; }

        [HtmlAttributeName("md-up")]
        public int? MediumAndUpSpan { get; set; }

        [HtmlAttributeName("sm-up")]
        public int? SmallAndUpSpan { get; set; }

        [HtmlAttributeName("md-down")]
        public int? MediumAndDownSpan { get; set; }

        [HtmlAttributeName("sm-down")]
        public int? SmallAndDownSpan { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "div";

            SetBootstrapSizingClasses(output, GetExtraSmallSpan(), GetSmallSpan(), GetMediumSpan(), GetLargeSpan());
        }

        private int? GetLargeSpan() => AllSpan ?? SmallAndUpSpan ?? MediumAndUpSpan ?? LargeSpan;

        private int? GetMediumSpan() => AllSpan ?? SmallAndUpSpan ?? MediumAndUpSpan ?? MediumAndDownSpan ?? MediumSpan;

        private int? GetSmallSpan() => AllSpan ?? SmallAndUpSpan ?? MediumAndDownSpan ?? SmallAndDownSpan ?? SmallSpan;

        private int? GetExtraSmallSpan() => AllSpan ?? MediumAndDownSpan ?? SmallAndDownSpan ?? ExtraSmallSpan;

        private void SetBootstrapSizingClasses(TagHelperOutput output, int? xs, int? sm, int? md, int? lg) {
            var classString = "";
            if (xs.HasValue) {
                classString += GenerateBootstrapSizeClass("xs", xs.Value);
            }
            if (sm.HasValue) {
                classString += GenerateBootstrapSizeClass("sm", sm.Value);
            }
            if (md.HasValue) {
                classString += GenerateBootstrapSizeClass("md", md.Value);
            }
            if (lg.HasValue) {
                classString += GenerateBootstrapSizeClass("lg", lg.Value);
            }
            output.Attributes.SetAttribute("class", classString);
        }

        private string GenerateBootstrapSizeClass(string prefix, int value) {
            if (value <= 0) {
                value = 1;
            } else if (value > 12) {
                value = 12;
            }
            return $"col-{prefix}-{value} ";
        }

    }

}