using PaperGate.Core.Entities.Categories;

namespace PaperGate.Web.ViewModels;

public class SidePanelViewModel
{
    public IReadOnlyList<CategoryInfo> Categories { get; set; } = default!;
}
