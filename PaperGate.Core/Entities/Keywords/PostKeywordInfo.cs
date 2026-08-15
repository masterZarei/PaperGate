using System.ComponentModel.DataAnnotations.Schema;

namespace PaperGate.Core.Entities.Keywords;
public class PostKeywordInfo : BaseEntity
{
    [ForeignKey(nameof(KeywordId))]
    public KeywordInfo Keyword { get; set; }
    public int KeywordId { get; set; }

    [ForeignKey(nameof(PostId))]
    public PostInfo Post { get; set; }
    public int PostId { get; set; }
}
