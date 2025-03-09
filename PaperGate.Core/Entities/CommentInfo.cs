using System.ComponentModel.DataAnnotations.Schema;

namespace PaperGate.Core.Entities;
public class CommentInfo : BaseEntity
{
    public string Content { get; set; }
    public bool IsVerified { get; set; }
    public string? Answer { get; set; }

    #region Relations
    public int PaperId { get; set; }
    [ForeignKey(nameof(PaperId))]
    public PaperInfo Paper { get; set; }

    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public UserInfo User { get; set; }
    #endregion
}