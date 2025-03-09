using PaperGate.Core.Entities;

namespace PaperGate.Core.DTOs
{
    public class PaperDetailsDto
    {
        public required string Title { get; set; }
        public required string Content { get; set; }

        #region Relations
        public string AuthorId { get; set; }
        public UserInfo Author { get; set; }
        public List<CommentInfo>? Comments { get; set; }
        #endregion

        public CommentInfo? Comment { get; set; }
    }
}
