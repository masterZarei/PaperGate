using System.ComponentModel.DataAnnotations;

namespace PaperGate.Core.Entities.Keywords;
public class KeywordInfo : BaseEntity
{
    [Required(ErrorMessage = "لطفا نام را وارد کنید")]
    [MaxLength(200, ErrorMessage = "نام نمی تواند از 200 کاراکتر بیشتر باشد")]
    public required string Title { get; set; }

    [MaxLength(200, ErrorMessage = "نام لاتین نمی تواند از 200 کاراکتر بیشتر باشد")]
    public string? EnglishTitle { get; set; }

    [MaxLength(2000, ErrorMessage = "توضیحات نمی تواند از 2000 کاراکتر بیشتر باشد")]
    public string? Description { get; set; }
}
