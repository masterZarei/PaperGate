// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PaperGate.Core.Entities;
using PaperGate.Web.Utilities.Helpers;
using ILogger = Serilog.ILogger;

namespace PaperGate.Web.Areas.Identity.Pages.Account;
public class LogoutModel : MyPageModel
{
    private readonly SignInManager<UserInfo> _signInManager;
    private readonly ILogger _logger;

    public LogoutModel(SignInManager<UserInfo> signInManager, ILogger logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            await _signInManager.SignOutAsync();
            ShowInfo("با موفقیت خارج شدید");
            return RedirectToIndex();
        }
        catch (Exception ex)
        {
            _logger.Error(ex,"LogOUT Page Failed.");
            return RedirectToIndex();
        }
        

    }
}