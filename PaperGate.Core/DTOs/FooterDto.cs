using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;

namespace PaperGate.Core.ViewModels;

public class FooterDto
{
    public IReadOnlyList<UsefulLinkInfo>? UsefulLinks { get; set; } = default!;
    public AboutUsPageDto AboutUs { get; set; }
}
