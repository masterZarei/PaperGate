using PaperGate.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PaperGate.Core.Entities.Template;
public class AboutUsInfo : BaseEntity, IDatabaseModel<AboutUsInfo>
{
    [Required(ErrorMessage = "لطفا اطلاعات صفحه درباره ما را وارد کنید")]
    public string Description { get; set; }

    [Required(ErrorMessage = "لطفا اطلاعات (لاتین) صفحه درباره ما را وارد کنید")]
    public string EnglishDescription { get; set; }

    public string? Image { get; set; }
}
