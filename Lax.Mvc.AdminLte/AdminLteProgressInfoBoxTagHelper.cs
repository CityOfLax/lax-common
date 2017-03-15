using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.AdminLte {

    [HtmlTargetElement("lte-progress-info-box")]
    public class AdminLteProgressInfoBoxTagHelper : TagHelper {

        [HtmlAttributeName("box-bg-color")]
        public string BoxBackgroundColor { get; set; } = "green";

        [HtmlAttributeName("box-icon")]
        public string BoxIcon { get; set; } = "";

        [HtmlAttributeName("box-title")]
        public string BoxTitle { get; set; } = "";

        [HtmlAttributeName("progress-numerator")]
        public int ProgressNumerator { get; set; } = 0;

        [HtmlAttributeName("progress-denominator")]
        public int ProgressDenominator { get; set; } = 0;
        
        public override void Process(TagHelperContext context, TagHelperOutput output) {

            int progress = (int)(((double)ProgressNumerator / (double)ProgressDenominator) * 100.0d);

            output.TagName = "div";

            var htmlContent = $"<span class='info-box-icon'><i class='fa fa-{BoxIcon}'></i></span>"
                + $"<div class='info-box-content'><span class='info-box-text'>{BoxTitle}</span>"
                + $"<span class='info-box-number'>{ProgressNumerator.ToString("F0")}/{ProgressDenominator.ToString("F0")}</span>"
                + $"<div class='progress'><div class='progress-bar' style='width: {progress.ToString("F0")}%;'></div>"
                + $"</div><div class='info-box-more'><span class='pull-right'>{progress.ToString("F0")}%</span>"
                + "</div></div>";

            output.Content.SetHtmlContent(htmlContent);

            output.Attributes.SetAttribute("class",$"info-box bg-{BoxBackgroundColor}");

            output.TagMode = TagMode.StartTagAndEndTag;

        }

    }

}