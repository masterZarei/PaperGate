namespace PaperGate.Core.Libraries.Convertors;
public static class StringConvertor
{
    public static string GetSummary(this string text, int maxLength = 150)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        if (text.Length <= maxLength)
            return text;

        // گرفتن 150 کاراکتر اول
        var substring = text.Substring(0, maxLength);

        // پیدا کردن آخرین فاصله در محدوده
        var lastSpaceIndex = substring.LastIndexOf(' ');

        // اگر فاصله‌ای وجود داشت، متن را تا آنجا ببُر
        if (lastSpaceIndex > 0)
            substring = substring.Substring(0, lastSpaceIndex);

        // افزودن سه نقطه در انتها
        return $"{substring}...";
    }
}
