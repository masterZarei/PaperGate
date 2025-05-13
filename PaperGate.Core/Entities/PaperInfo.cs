using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Entities.Ketwords;
using PaperGate.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperGate.Core.Entities;

public class PaperInfo : BaseEntity, IDatabaseModel<PaperInfo>, ISoftDeleteDatabaseModel
{
    [Required(ErrorMessage = "لطفا موضوع مقاله را وارد کنید")]
    [MaxLength(150, ErrorMessage = "موضوع مقاله نمی تواند از 150 کاراکتر بیشتر باشد")]
    public required string Title { get; set; }
    public string? Slug { get; set; }

    [Required(ErrorMessage = "لطفا محتوای مقاله را وارد کنید")]
    public required string Content { get; set; }
    public bool IsActive { get; set; } = false;

    public string? Picture { get; set; }
    [Required(ErrorMessage = "لطفا معرفی کوتاه مقاله را وارد کنید")]
    public string Summary { get; set; }

    #region Relations
    public string AuthorId { get; set; }
    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public UserInfo Author { get; set; }

    public ICollection<PaperCategoryInfo> Categories { get; } = [];
    public ICollection<PaperKeywordInfo> Keywords { get; } = [];
    #endregion
}
