using PaperGate.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PaperGate.Core.Entities.Categories;
public class CategoryInfo : BaseEntity, IDatabaseModel<CategoryInfo>
{
    [Required(ErrorMessage = "لطفا نام دسته بندی را وارد کنید")]
    [MaxLength(200, ErrorMessage = "نام دسته بندی نمی تواند از 200 کاراکتر بیشتر باشد")]
    public required string Title { get; set; }

    [MaxLength(2000, ErrorMessage = "نام دسته بندی نمی تواند از 2000 کاراکتر بیشتر باشد")]
    public string? Description { get; set; }
}
