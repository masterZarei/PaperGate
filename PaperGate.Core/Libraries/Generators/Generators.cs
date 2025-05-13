using Microsoft.EntityFrameworkCore.Infrastructure;
using PaperGate.Core.Entities;
using System.Text.RegularExpressions;

namespace PaperGate.Core.Libraries.Generators;
public static class NameGenerator
{
    public static string FilenameGenerate(string fileName, string extention)
    {
        return $"{Regex.Replace(fileName, @"[^a-zA-Z0-9\u0600-\u06FF]", "-").ToUpper()[..Math.Min(50, fileName.Length)]}{GenerateUniqueName}{extention}";
    }
    public static string GenerateUniqueName => $"{DateTime.Now.ToBinary()}";
}
public static class CodeGenerator
{
    public static int GenerateUniqueCode => new Random().Next(10000000, 99999999);
}
public static class SlugGenerator
{
    public static string GenerateSlug(string input, List<PaperInfo>? papers)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // 1. حذف کاراکترهای غیرمجاز
        string slug = Regex.Replace(input, @"[^ء-یa-zA-Z0-9\s-]", "");

        // 2. جایگزینی فاصله‌ها با خط تیره
        slug = Regex.Replace(slug, @"\s+", "-");

        // 3. حذف خط تیره‌های اضافه از ابتدا و انتهای متن
        slug = slug.Trim('-');

        // 4. unique کردن slug
        if (papers.Any(b => b.Slug == slug))
        {
            int count = 2;
            while (papers.Select(bp => bp.Slug).Contains($"{slug}-{count}"))
            {
                count++;
            }
            slug = $"{slug}-{count}";
        }

        return slug.ToLowerInvariant();
    }
}