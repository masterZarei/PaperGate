using PaperGate.Core.Entities;
using System.Text.RegularExpressions;

namespace PaperGate.Core.Libraries.Generators;
public static class NameGenerator
{
    public static string FilenameGenerate(string fileName, string extension)
    {
        string cleanedName = Regex.Replace(fileName, @"[^a-zA-Z0-9\u0600-\u06FF]", "-").ToUpperInvariant();
        if (cleanedName.Length > 50)
            cleanedName = cleanedName[..50];

        return $"{cleanedName}{GenerateUniqueName}{extension}";
    }
    public static string GenerateUniqueName => $"{Guid.NewGuid():N}";
}
public static class SlugGenerator
{
    public static string GenerateSlug(string input, List<PostInfo>? papers)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        string slug = Regex.Replace(input, @"[^ء-یa-zA-Z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = slug.Trim('-');

        if (papers is not null && papers.Any(b => b.Slug == slug))
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
