using Ganss.Xss;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace PaperGate.Web.Utilities.Helpers.TagHelpers;

[HtmlTargetElement("SafeHtml")]
public class SafeHtmlTagHelper : TagHelper
{
    public string Content { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var sanitizer = new HtmlSanitizer();
        var sanitizedContent = sanitizer.Sanitize(Content);

        output.TagName = null; // حذف تگ والد
        output.Content.SetHtmlContent(sanitizedContent);
    }
}