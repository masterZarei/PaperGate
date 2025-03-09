using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using PaperGate.Core.Interfaces;

namespace PaperGate.Core.Entities;

public class UserInfo : IdentityUser, IDatabaseModel<UserInfo>, ISoftDeleteDatabaseModel
{

    [MaxLength(255, ErrorMessage = "نام کاربر نمی تواند از 255 کاراکتر بیشتر باشد")]
    public string? Name { get; set; }

    [MaxLength(500, ErrorMessage = "نام خانوادگی کاربر نمی تواند از 500 کاراکتر بیشتر باشد")]
    public string? LastName { get; set; }

    [MaxLength(9, ErrorMessage = "شماره دانشجویی کاربر نمی تواند از 9 کاراکتر بیشتر باشد")]
    public int StudentNumber { get; set; }

    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
}