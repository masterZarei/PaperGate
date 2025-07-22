// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;
using PaperGate.Core.Libraries.StaticValues;
using PaperGate.Infra.Data;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : MyPageModel
    {
        private readonly SignInManager<UserInfo> _signInManager;
        private readonly UserManager<UserInfo> _userManager;
        private readonly ILogger _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IUserStore<UserInfo> _userStore;
        /*        private readonly IUserEmailStore<UserInfo> _emailStore;
                private readonly IEmailSender _emailSender;*/

        public RegisterModel(
            UserManager<UserInfo> userManager,
            IUserStore<UserInfo> userStore,
            SignInManager<UserInfo> signInManager,
            ILogger logger,
            /*IEmailSender emailSender*/
            RoleManager<IdentityRole> roleManager,
            AppDbContext context)
        {
            _userStore = userStore;
            /*
            _emailStore = GetEmailStore();
            _emailSender = emailSender;*/
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public RegisterDto RegisterDto { get; set; } = default!;



        public async Task OnGetAsync(string returnUrl = null)
        {
            RegisterDto = new()
            {
                ReturnUrl = returnUrl
            };
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.NationalCode = RegisterDto.NationalCode;
                user.UserName = RegisterDto.NationalCode;
                user.Name = RegisterDto.Name;
                user.LastName = RegisterDto.LastName;

                await _userStore.SetUserNameAsync(user, RegisterDto.NationalCode, CancellationToken.None);
                /*                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                */
                var result = await _userManager.CreateAsync(user, RegisterDto.Password);

                if (result.Succeeded)
                {
                    await AssignRole(user);
                    _logger.Information("User created a new account with password.");
                    /*var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
*/
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = RegisterDto.NationalCode, returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        ShowSuccess("حساب کاربری شما با موفقیت ایجاد شد");
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private UserInfo CreateUser()
        {
            try
            {
                return Activator.CreateInstance<UserInfo>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserInfo)}'. " +
                    $"Ensure that '{nameof(UserInfo)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        private async Task AssignRole(UserInfo user)
        {
            Guard.Against.Null(user);
            if (!await _roleManager.RoleExistsAsync(Roles.AdminEndUser))
                await _roleManager.CreateAsync(new IdentityRole(Roles.AdminEndUser));

            if (_context.Users.ToList().Count == 1)
                await _userManager.AddToRoleAsync(user, Roles.AdminEndUser);

          /*  else
                await _userManager.AddToRoleAsync(user, Roles.StudentEndUser);*/
        }
        /*private IUserEmailStore<UserInfo> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<UserInfo>)_userStore;
        }*/
    }
}
