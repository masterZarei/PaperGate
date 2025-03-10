using PaperGate.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperGate.Core.Entities;
public class CommentInfo : BaseEntity, IDatabaseModel<CommentInfo>, ISoftDeleteDatabaseModel
{
    public string Content { get; set; }
    public bool IsVerified { get; set; }
    public string? Answer { get; set; }

    #region Relations
    public int? PaperId { get; set; }
    [ForeignKey(nameof(PaperId))]
    public virtual PaperInfo? Paper { get; set; }

    public string? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual UserInfo? User { get; set; }
    public bool IsDeleted { get; set; }
    #endregion
}