namespace PaperGate.Core.Entities;
public class ContactWayInfo : BaseEntity
{
    public string Title { get; set; }
    public string? EnglishTitle { get; set; } = string.Empty;
    public string? Link { get; set; }
    public string? Icon { get; set; }
    public bool IsLink { get; set; }

}
