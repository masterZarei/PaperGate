// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;
using PaperGate.Infra.Data;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;


namespace PaperGate.Web.Areas.Identity.Pages.Account
{
    public class LoginModel : MyPageModel
    {
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly ILogger _logger;
        private readonly AppDbContext _context;

        public LoginModel(SignInManager<UserInfo> signInManager, ILogger logger, AppDbContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }
        [BindProperty]
        public LoginDto LoginDto { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync();

            LoginDto = new()
            {
                ReturnUrl = returnUrl
            };
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(LoginDto.NationalCode, LoginDto.Password, LoginDto.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == LoginDto.NationalCode);
                    if (user.IsDeleted)
                    {
                        ShowError(ErrorMessages.CUSTOM, "حساب کاربری شما حذف شده است!");
                        HttpContext.Session.Clear(); // پاک‌سازی سشن
                        await _signInManager.SignOutAsync();
                        return Redirect("/");
                    }
                    _logger.Information($"User {LoginDto.NationalCode} logged in.");
                    ShowSuccess("با موفقیت وارد شدید");
                    return LocalRedirect(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.Warning($"User {LoginDto.NationalCode} account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "ورود نامعتبر است!");
                    ShowError(ErrorMessages.CUSTOM, "ورود نامعتبر است!");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
