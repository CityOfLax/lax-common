using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lax.Mvc.Bootstrap {

    public abstract class SimpleBootstrapTagHelper : TagHelper {
        
        protected abstract string TagName { get; }

        protected abstract string ClassAttributes { get; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {

            output.TagName = TagName;

            output.Attributes.SetAttribute("class", ClassAttributes);

        }

    }

}