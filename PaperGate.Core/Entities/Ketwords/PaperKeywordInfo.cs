using PaperGate.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperGate.Core.Entities.Ketwords;
public class PaperKeywordInfo : BaseEntity, IDatabaseModel<PaperKeywordInfo>
{
    [ForeignKey(nameof(KeywordId))]
    public KeywordInfo Keyword { get; set; }
    public int KeywordId { get; set; }

    [ForeignKey(nameof(PaperId))]
    public PostInfo Paper { get; set; }
    public int PaperId { get; set; }
}
