using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Categories;

namespace PaperGate.Web.ViewModels;

public class IndexDto
{
    public IReadOnlyList<PostInfo>? Sliders { get; set; }
   // public BannerInfo? Banner { get; set; }
    //public IReadOnlyList<PostInfo>? Posts { get; set; }

    public IReadOnlyList<CategoryInfo>? Categories { get; set; }

    public string? AddOnHeadHtml { get; set; }
    public string? AddOnBodyHtml { get; set; }
    public string? AddOnAfterBodyHtml { get; set; }
}
