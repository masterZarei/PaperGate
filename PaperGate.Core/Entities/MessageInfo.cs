using System.ComponentModel.DataAnnotations;

namespace PaperGate.Core.Entities;
public class MessageInfo : BaseEntity
{
    [Required(ErrorMessage = "لطفا نام و نام خانوادگی خود را وارد کنید")]
    [MaxLength(150, ErrorMessage = "حداکثر 150 کاراکتر وارد کنید")]
    public string SendersName { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "لطفا آدرس ایمیل خود را وارد کنید")]
    public string SendersEmail { get; set; }

   // public string? SendersPhoneNumber { get; set; }

    [Required(ErrorMessage = "لطفا محتویات پیام خود را وارد کنید")]
    [MaxLength(100000, ErrorMessage = "لطفا حداکثر 100000 کاراکتر وارد کنید")]
    public string Content { get; set; }

    public bool Read { get; set; }
}
