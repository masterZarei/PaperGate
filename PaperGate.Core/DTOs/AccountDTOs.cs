using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PaperGate.Core.DTOs;
public class UserBaseDto
{
    [Required(ErrorMessage = "لطفا نام خود را وارد کنید")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "لطفا نام خانوادگی خود را وارد کنید")]
    public string? LastName { get; set; }

    [MaxLength(10, ErrorMessage = "کد ملی کاربر نمی تواند از 10 کاراکتر بیشتر باشد")]
    public string? NationalCode { get; set; }

}
public class LoginDto
{
    [MaxLength(10, ErrorMessage = "کد ملی کاربر نمی تواند از 10 کاراکتر بیشتر باشد")]
    public string? NationalCode { get; set; }

    [Required(ErrorMessage = "لطفا رمز عبور خود را وارد کنید")]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }
}
public class RegisterDto : UserBaseDto
{
    [Required(ErrorMessage = "لطفا رمز عبور خود را وارد کنید")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "لطفا تکرار رمز عبور خود را وارد کنید")]
    [Compare(nameof(Password), ErrorMessage = "رمز عبور با تکرار آن همخوانی ندارد")]
    public string? ConfirmPassword { get; set; }
}
public class PersonalInfoDto : UserBaseDto
{
    public DateTime DateCreated { get; set; }
}
public class ForgottonChangePasswordDto
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "لطفا رمز عبور جدید را وارد کنید")]
    public string? NewPassword { get; set; }

    [Required(ErrorMessage = "لطفا تکرار رمز عبور خود را وارد کنید")]
    [Compare(nameof(NewPassword), ErrorMessage = "رمز عبور با تکرار آن همخوانی ندارد")]
    public string? ConfirmNewPassword { get; set; }
}
public class UserChangePasswordDto
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "لطفا رمز عبور را وارد کنید")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "لطفا رمز عبور جدید را وارد کنید")]
    public string? NewPassword { get; set; }

    [Required(ErrorMessage = "لطفا تکرار رمز عبور خود را وارد کنید")]
    [Compare(nameof(NewPassword), ErrorMessage = "رمز عبور با تکرار آن همخوانی ندارد")]
    public string? ConfirmNewPassword { get; set; }

    public string? FullName { get; set; }

    public PasswordChanger PasswordChanger { get; set; } = PasswordChanger.LoggedInUser;
}
public class AdminChangePasswordDto
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "لطفا رمز عبور جدید را وارد کنید")]
    public string? NewPassword { get; set; }

    [Required(ErrorMessage = "لطفا تکرار رمز عبور خود را وارد کنید")]
    [Compare(nameof(NewPassword), ErrorMessage = "رمز عبور با تکرار آن همخوانی ندارد")]
    public string? ConfirmNewPassword { get; set; }

    public string? FullName { get; set; }

    public PasswordChanger PasswordChanger { get; set; } = PasswordChanger.Admin;
}
public class UserListDto : UserBaseDto
{
    public string? Id { get; set; }
    public string? Role { get; set; }
    public string? Username { get; set; }
    public string NationalCode { get; set; }

}
public class User_ED_Dto : UserBaseDto
{
    public string? Id { get; set; }
    public bool IsActive { get; set; } = true;
    public string NationalCode { get; set; }

    #region Role List
    public SelectList? RolesList { get; set; }
    public string? Role { get; set; }

    #endregion
}
public class UserCreateDto : UserBaseDto
{
    public string? Password { get; set; }
    public string NationalCode { get; set; }

    #region Role List
    public SelectList? RolesList { get; set; }
    public string? Role { get; set; }

    #endregion

}
public class UserDeleteDto : UserBaseDto
{
    public string? Id { get; set; }
}
public enum PasswordChanger
{
    LoggedInUser,
    Admin
}
