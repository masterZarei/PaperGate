// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PaperGate.Core.Entities;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Areas.Identity.Pages.Account;
public class LogoutModel : MyPageModel
{
    private readonly SignInManager<UserInfo> _signInManager;
    private readonly ILogger _logger;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public LogoutModel(SignInManager<UserInfo> signInManager, ILogger logger, IStringLocalizer<SharedResources> localizer)
    {
        _signInManager = signInManager;
        _logger = logger;
        _localizer = localizer;
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            await _signInManager.SignOutAsync();
            ShowInfo(_localizer["LoggedOutSuccessfully"]);
            return RedirectToIndex();
        }
        catch (Exception ex)
        {
            _logger.Error(ex,"LogOUT Page Failed.");
            return RedirectToIndex();
        }
        

    }
}