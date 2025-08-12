using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Entities.Ketwords;
using System.ComponentModel.DataAnnotations;

namespace PaperGate.Core.DTOs;

public class PostCreateDto
{
    /*
     [Required(ErrorMessage ="لطفا موضوع بلاگ را وارد کنید")]
    [MaxLength(150,ErrorMessage ="موضوع بلاگ نمی تواند از 150 کاراکتر بیشتر باشد")]
    public string Title { get; set; }
    public string? Slug { get; set; }
    [Required(ErrorMessage = "لطفا محتوای بلاگ را وارد کنید")]
    public string Content { get; set; }
    public string? Picture { get; set; }
    [Required(ErrorMessage = "لطفا خلاصه بلاگ را وارد کنید")]
    public string Summary { get; set; }
    public bool IsDeleted { get; set; }

    #region Relations

    public string? AuthorId { get; set; }
    [ForeignKey(nameof(AuthorId))]
    public UserInfo? Author { get; set; }
    #endregion
     */
    [Required(ErrorMessage = "لطفا موضوع بلاگ را وارد کنید")]
    [MaxLength(150, ErrorMessage = "موضوع بلاگ نمی تواند از 150 کاراکتر بیشتر باشد")]
    public string Title { get; set; }
    [Required(ErrorMessage = "لطفا محتوای بلاگ را وارد کنید")]
    public string Content { get; set; }
    [Required(ErrorMessage = "لطفا خلاصه بلاگ را وارد کنید")]
    public string Summary { get; set; }
    public string EnglishContent { get; set; } = default!;

    #region Upload Props
    public string? Picture { get; set; }
    public IFormFile? FileUpload { get; set; } = default!;
    #endregion
}
public class PostEditDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "لطفا موضوع بلاگ را وارد کنید")]
    [MaxLength(150, ErrorMessage = "موضوع بلاگ نمی تواند از 150 کاراکتر بیشتر باشد")]
    public string Title { get; set; }

    [Required(ErrorMessage = "لطفا محتوای بلاگ را وارد کنید")]
    public string Content { get; set; }
    [Required(ErrorMessage = "لطفا خلاصه بلاگ را وارد کنید")]
    public string Summary { get; set; }
    public string EnglishContent { get; set; } = default!;
    public bool IsActive { get; set; }
    public bool ShowOnSlider { get; set; }
    public string? AuthorId { get; set; }
    public string? Slug { get; set; }
    public int CategoryId { get; set; }


    #region Upload Props
    public string? Picture { get; set; }
    public IFormFile? FileUpload { get; set; }
    #endregion

    #region Category List
    public List<CategoryInfo>? AvailableCategories { get; set; }
    public List<CategoryInfo>? PostCategories { get; set; }
    public SelectList? CategoryList { get; set; }
    public string? SelectedCategory { get; set; }
    #endregion

    #region Keyword List
    public List<KeywordInfo>? AvailableKeywords { get; set; }
    public List<KeywordInfo>? PostKeywords { get; set; }
    public SelectList? KeywordList { get; set; }
    public string? SelectedKeyword { get; set; }
    #endregion
}
public class PostListDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Picture { get; set; }
    public DateTime CreatedOn { get; set; }
    public UserInfo? Author { get; set; }

}
public class PostDeleteDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Picture { get; set; }
    public DateTime CreatedOn { get; set; }

}
public class PostDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
    public string EnglishContent { get; set; } = default!;
    public string? Picture { get; set; }
    public int CategoryId { get; set; }

    public DateTime CreatedOn { get; set; }
    public IReadOnlyList<PostKeywordInfo>? PostKeywords { get; set; }
}
public class PublicPostDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string EnglishContent { get; set; } = default!;
    public string Summary { get; set; }
    public string? Picture { get; set; }
    public IReadOnlyList<PostKeywordInfo>? PostKeywords { get; set; }
    public IReadOnlyList<PostInfo>? LatestPosts { get; set; }
    public UserInfo? Author { get; set; }
    public string? Slug { get; set; }
    public DateTime CreatedOn { get; set; }
    public Language CurrentLanguage { get; set; }


}
public enum Language
{
    Persian,
    English
}
public class AllPostsDto
{
    public IReadOnlyList<PostInfo>? Posts { get; set; }
    public string? PostTitle { get; set; } = string.Empty;
}