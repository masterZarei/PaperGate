using System.ComponentModel.DataAnnotations;

namespace PaperGate.Web.ViewModels;

public class AboutUsEditDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "لطفا اطلاعات صفحه درباره ما را وارد کنید")]
    public string Description { get; set; }
    public string? Image { get; set; }

    public IFormFile? ImageUpload { get; set; }
}
public class AboutUsPageDto
{
    public string? Description { get; set; }
    public string? Image { get; set; }
}
