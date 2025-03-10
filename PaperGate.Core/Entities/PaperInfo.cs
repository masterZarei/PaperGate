using PaperGate.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperGate.Core.Entities;

public class PaperInfo : BaseEntity, IDatabaseModel<PaperInfo>, ISoftDeleteDatabaseModel
{
    public required string Title { get; set; }
    public required string Content { get; set; }

    #region Relations
    public string AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public UserInfo Author { get; set; }
    public List<CommentInfo>? Comments { get; set; }
    public bool IsDeleted { get; set; }
    #endregion
}
