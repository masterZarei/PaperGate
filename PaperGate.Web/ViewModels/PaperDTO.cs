using Microsoft.AspNetCore.Mvc.Rendering;
using PaperGate.Core.Entities;
using PaperGate.Core.Entities.Categories;
using PaperGate.Core.Entities.Ketwords;
using System.ComponentModel.DataAnnotations;

namespace PaperGate.Web.ViewModels;

public class PaperCreateDto
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
    #region Upload Props
    public string? Picture { get; set; }
    public IFormFile? FileUpload { get; set; } = default!;
    #endregion
}
public class PaperEditDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "لطفا موضوع بلاگ را وارد کنید")]
    [MaxLength(150, ErrorMessage = "موضوع بلاگ نمی تواند از 150 کاراکتر بیشتر باشد")]
    public string Title { get; set; }

    [Required(ErrorMessage = "لطفا محتوای بلاگ را وارد کنید")]
    public string Content { get; set; }
    [Required(ErrorMessage = "لطفا خلاصه بلاگ را وارد کنید")]
    public string Summary { get; set; }
    public bool IsActive { get; set; }
    public string? AuthorId { get; set; }
    public string? Slug { get; set; }


    #region Upload Props
    public string? Picture { get; set; }
    public IFormFile? FileUpload { get; set; }
    #endregion

    #region Category List
    public List<CategoryInfo>? AvailableCategories { get; set; }
    public List<CategoryInfo>? PaperCategories { get; set; }
    public SelectList? CategoryList { get; set; }
    public string? SelectedCategory { get; set; }
    #endregion

    #region Keyword List
    public List<KeywordInfo>? AvailableKeywords { get; set; }
    public List<KeywordInfo>? PaperKeywords { get; set; }
    public SelectList? KeywordList { get; set; }
    public string? SelectedKeyword { get; set; }
    #endregion
}
public class PaperListDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Picture { get; set; }
    public DateTime CreatedOn { get; set; }
    public UserInfo? Author { get; set; }

}
public class PaperDeleteDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Picture { get; set; }
    public DateTime CreatedOn { get; set; }

}
public class PaperDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
    public string? Picture { get; set; }

    public DateTime CreatedOn { get; set; }
    public IReadOnlyList<PaperCategoryInfo>? PaperCategories { get; set; }
    public IReadOnlyList<PaperKeywordInfo>? PaperKeywords { get; set; }
}
public class PublicPaperDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Summary { get; set; }
    public string? Picture { get; set; }
    public IReadOnlyList<PaperCategoryInfo>? PaperCategories { get; set; }
    public IReadOnlyList<PaperKeywordInfo>? PaperKeywords { get; set; }
    public IReadOnlyList<PaperInfo>? LatestPapers { get; set; }
    public UserInfo? Author { get; set; }
    public string? Slug { get; set; }
    public DateTime CreatedOn { get; set; }


}
public class PaperCardDto
{
    public string? Picture { get; set; }
    public string Title { get; set; }
    public UserInfo? Author { get; set; }


}
public class AllPapersDto
{
    public IReadOnlyList<PaperInfo>? Papers { get; set; }
    public string? PaperTitle { get; set; } = string.Empty;
    public UserInfo? Author { get; set; }


}