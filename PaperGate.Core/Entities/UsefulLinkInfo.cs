using PaperGate.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PaperGate.Core.Entities;
public class UsefulLinkInfo : BaseEntity, IDatabaseModel<UsefulLinkInfo>
{
    [Required(ErrorMessage = "لطفا نام را وارد کنید")]
    [MaxLength(200, ErrorMessage = "نام نمی تواند از 200 کاراکتر بیشتر باشد")]
    public required string Title { get; set; }

    [MaxLength(2000, ErrorMessage = "لینک نمی تواند از 2000 کاراکتر بیشتر باشد")]
    public string? Link { get; set; }
}