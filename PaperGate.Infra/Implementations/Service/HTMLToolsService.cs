using Ganss.Xss;
using PaperGate.Core.Interfaces.Services;

namespace PaperGate.Infra.Implementations.Service;
public class HTMLToolsService : IHTMLToolsService
{
    private readonly HtmlSanitizer _sanitizer;

    public HTMLToolsService()
    {
        // تنظیمات سفارشی برای اجازه دادن به تگ‌ها و ویژگی‌ها
        _sanitizer = new HtmlSanitizer();

        // تگ‌های مرتبط با متن
        _sanitizer.AllowedTags.Add("p");  // پاراگراف
        _sanitizer.AllowedTags.Add("a");  // لینک
        _sanitizer.AllowedTags.Add("img"); // تصویر
        _sanitizer.AllowedTags.Add("strong"); // Bold
        _sanitizer.AllowedTags.Add("em"); // Italic
        _sanitizer.AllowedTags.Add("u"); // Underline
        _sanitizer.AllowedTags.Add("strike"); // خط‌زده
        _sanitizer.AllowedTags.Add("span"); // برای استایل‌های Inline

        // تگ‌های لیست
        _sanitizer.AllowedTags.Add("ul");
        _sanitizer.AllowedTags.Add("ol");
        _sanitizer.AllowedTags.Add("li");

        // ویژگی‌های مجاز
        _sanitizer.AllowedAttributes.Add("src"); // خاصیت src برای img
        _sanitizer.AllowedAttributes.Add("href"); // خاصیت href برای لینک
        _sanitizer.AllowedAttributes.Add("style"); // خاصیت style برای inline CSS
        _sanitizer.AllowedAttributes.Add("class"); // خاصیت class برای کلاس‌های CSS
        _sanitizer.AllowedAttributes.Add("alt"); // خاصیت alt برای تصاویر
        _sanitizer.AllowedAttributes.Add("title"); // خاصیت title
    }

    public string SanitizeContent(string htmlContent)
    {
        return _sanitizer.Sanitize(htmlContent);
    }
}
