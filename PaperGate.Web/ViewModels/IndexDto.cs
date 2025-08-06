using PaperGate.Core.Entities;

namespace PaperGate.Web.ViewModels;

public class IndexDto
{
    public IReadOnlyList<PostInfo>? Sliders { get; set; }
   // public BannerInfo? Banner { get; set; }
    public IReadOnlyList<PostInfo>? Posts { get; set; }

    public string? AddOnHeadHtml { get; set; }
    public string? AddOnBodyHtml { get; set; }
    public string? AddOnAfterBodyHtml { get; set; }
}
