using PaperGate.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperGate.Core.Entities.Categories;
public class PaperCategoryInfo : BaseEntity, IDatabaseModel<PaperCategoryInfo>
{
    [ForeignKey(nameof(CategoryId))]
    public CategoryInfo Category { get; set; }
    public int CategoryId { get; set; }

    [ForeignKey(nameof(PaperId))]
    public PaperInfo Paper { get; set; }
    public int PaperId { get; set; }
}
