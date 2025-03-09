using System.ComponentModel.DataAnnotations.Schema;

namespace PaperGate.Core.Entities;

public class PaperInfo : BaseEntity
{
    public required string Title { get; set; }
    public required string Content { get; set; }

    #region Relations
    public string AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public UserInfo Author { get; set; }
    public List<CommentInfo>? Comments { get; set; }
    #endregion
}
