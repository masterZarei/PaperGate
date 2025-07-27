using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperGate.Core.Entities;

public class PostInfo : BaseEntity, IDatabaseModel<PostInfo>, ISoftDeleteDatabaseModel
{
    [Required(ErrorMessage = "لطفا موضوع پست را وارد کنید")]
    [MaxLength(150, ErrorMessage = "موضوع پست نمی تواند از 150 کاراکتر بیشتر باشد")]
    public required string Title { get; set; }
    public string? Slug { get; set; }

    [Required(ErrorMessage = "لطفا محتوای پست را وارد کنید")]
    public required string Content { get; set; }
    public bool IsActive { get; set; } = false;

    public string? Picture { get; set; }
    [Required(ErrorMessage = "لطفا معرفی کوتاه پست را وارد کنید")]
    public string Summary { get; set; }

    public int CategoryId { get; set; }

    public bool ShowOnSlider { get; set; }

    #region Relations
    public string AuthorId { get; set; }
    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public UserInfo Author { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public CategoryInfo Category { get; } = default!;
    public ICollection<PaperKeywordInfo> Keywords { get; } = [];
    #endregion
}
