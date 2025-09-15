namespace PaperGate.Core.Entities;
public class ContactWayInfo : BaseEntity
{
    public string Title { get; set; }
    public string? Link { get; set; }
    public string? EnglishLink { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public bool IsLink { get; set; }

}
