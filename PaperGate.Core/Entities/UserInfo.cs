using Microsoft.AspNetCore.Identity;
using PaperGate.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PaperGate.Core.Entities;

public class UserInfo : IdentityUser, IDatabaseModel<UserInfo>, ISoftDeleteDatabaseModel
{

    [MaxLength(255, ErrorMessage = "نام کاربر نمی تواند از 255 کاراکتر بیشتر باشد")]
    public string? Name { get; set; }

    [MaxLength(500, ErrorMessage = "نام خانوادگی کاربر نمی تواند از 500 کاراکتر بیشتر باشد")]
    public string? LastName { get; set; }

    [MaxLength(10, ErrorMessage = "کد ملی کاربر نمی تواند از 10 کاراکتر بیشتر باشد")]
    public string NationalCode { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
}