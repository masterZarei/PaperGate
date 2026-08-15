using Ardalis.GuardClauses;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces.Services;
using PaperGate.Core.Libraries.StaticValues;
using PaperGate.Core.Validations.UserDtosValidator;
using PaperGate.Infra.Data;
using PaperGate.Infra.Implementations.Repositories;
using PaperGate.Shared.ReturnTypes;
using Serilog;
using System.Text.Json;

namespace PaperGate.Infra.Implementations.Service;
public class UserService : GenericRepository<UserInfo>, IUserService
{
    private readonly AppDbContext _db;
    private readonly IUserStore<UserInfo> _userStore;
    private readonly UserManager<UserInfo> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<UserInfo> _signInManager;
    private readonly ILogger _myLogger;
    private readonly IMapper _mapper;

    public UserService(AppDbContext bookDb,
        RoleManager<IdentityRole> roleManager,
        IUserStore<UserInfo> userStore,
        UserManager<UserInfo> userManager,
        SignInManager<UserInfo> signInManager,
        ILogger myLogger,
        IMapper mapper) : base(bookDb, myLogger)
    {
        _db = bookDb;
        _userStore = userStore;
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _myLogger = myLogger;
        _mapper = mapper;
    }

    public async Task<UserInfo?> GetUserById(string userId)
    {
        try
        {
            Guard.Against.NullOrEmpty(userId);
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            UserInfo user = await GetAsync(a => a.Id == userId && !a.IsDeleted);
            return user;
        }
        catch (Exception ex)
        {
            _myLogger.Warning(ex, "UserService/GetUserById");
            return null;
        }
    }

    public async Task<UserInfo?> GetUserByUsername(string userName)
    {
        try
        {
            Guard.Against.NullOrEmpty(userName);
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            return await GetAsync(a => a.UserName == userName && !a.IsDeleted);
        }
        catch (Exception ex)
        {
            _myLogger.Warning(ex, "UserService/GetUserByUsername");
            return null;
        }
    }

    public async Task<string?> GetUsersRole(string userId)
    {
        try
        {
            Guard.Against.NullOrEmpty(userId);
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            UserInfo user = await GetAsync(a => a.Id == userId && !a.IsDeleted);
            if (user is null)
                return null;

            return string.Join(",", await _userManager.GetRolesAsync(user));
        }
        catch (Exception ex)
        {
            _myLogger.Warning(ex, "UserService/GetUsersRole");
            return null;
        }
    }

    public async Task<TaskResult> LoginUser(LoginDto login)
    {
        var taskResult = new TaskResult();
        try
        {
            Guard.Against.Null(login);

            LoginDtoValidation validator = new();
            var validation = validator.Validate(login);
            if (validation.IsValid is false)
            {
                foreach (var failure in validation.Errors)
                    taskResult.AddError(failure.ErrorMessage);
                return taskResult;
            }

            UserInfo User = await GetAsync(a => a.NationalCode == login.NationalCode);
            if (User is null || User.IsDeleted)
            {
                taskResult.AddError("کاربری با این مشخصات موجود نیست");
                return taskResult;
            }

            var result = await _signInManager.PasswordSignInAsync(login.NationalCode, login.Password, login.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                taskResult.Result = User;
                taskResult.Succeeded = true;
                return taskResult;
            }
            else if (result.IsLockedOut)
            {
                taskResult.AddError($"کاربر {User.NationalCode} قفل شده است!");
                return taskResult;
            }

            taskResult.AddError("شماره همراه یا رمز عبور نامعتبر است");
            return taskResult;
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/LoginUser");
            taskResult.AddError("خطایی رخ داد!");
            taskResult.AddError("لطفا بعدا دوباره امتحان کنید!");
            return taskResult;
        }
    }

    public async Task<TaskResult> RegisterUser(RegisterDto register)
    {
        var taskResult = new TaskResult();
        try
        {
            Guard.Against.Null(register);

            RegisterDtoValidation validator = new();
            var validation = validator.Validate(register);
            if (validation.IsValid is false)
            {
                foreach (var failure in validation.Errors)
                    taskResult.AddError(failure.ErrorMessage);
                return taskResult;
            }

            var user = Activator.CreateInstance<UserInfo>();

            user.Name = register.Name;
            user.LastName = register.LastName;
            user.NationalCode = register.NationalCode;

            await _userStore.SetUserNameAsync(user, register.NationalCode, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                await EnsureRolesExist();

                await _signInManager.SignInAsync(user, isPersistent: false);
                taskResult.Succeeded = true;
                return taskResult;
            }

            foreach (var error in result.Errors)
                taskResult.AddError(error.Description);

            return taskResult;
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/RegisterUser");
            taskResult.AddError("خطایی رخ داد!");
            taskResult.AddError("لطفا بعدا دوباره امتحان کنید!");
            return taskResult;
        }
    }

    private async Task EnsureRolesExist()
    {
        if (!await _roleManager.RoleExistsAsync(Roles.AdminEndUser))
            await _roleManager.CreateAsync(new IdentityRole(Roles.AdminEndUser));
    }

    public async Task<TaskResult> SoftRemoveUser(UserDeleteDto deleteDto)
    {
        var taskResult = new TaskResult();
        try
        {
            Guard.Against.Null(deleteDto);
            if (string.IsNullOrWhiteSpace(deleteDto.Id))
            {
                taskResult.AddError("شناسه کاربر نمی تواند خالی باشد.");
                return taskResult;
            }

            UserInfo? user = await GetUserById(deleteDto.Id);
            if (user is null)
            {
                taskResult.AddError("کاربری با این مشخصات موجود نیست");
                return taskResult;
            }

            user.IsDeleted = true;

            await UpdateAsync(user);
            await _db.SaveChangesAsync();
            taskResult.Succeeded = true;
            return taskResult;
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/SoftRemoveUser");
            taskResult.AddError("خطایی رخ داد!");
            taskResult.AddError("لطفا بعدا دوباره امتحان کنید!");
            return taskResult;
        }
    }

    public async Task<TaskResult> CreateUser(UserCreateDto createDto)
    {
        var taskResult = new TaskResult();
        try
        {
            Guard.Against.Null(createDto);

            UserCreateDtoValidation validator = new();
            var validation = validator.Validate(createDto);
            if (validation.IsValid is false)
            {
                foreach (var failure in validation.Errors)
                    taskResult.AddError(failure.ErrorMessage);
                return taskResult;
            }

            var user = Activator.CreateInstance<UserInfo>();
            user = _mapper.Map<UserInfo>(createDto);
            await _userStore.SetUserNameAsync(user, createDto.NationalCode, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, createDto.Password);

            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(createDto.Role) && !await _roleManager.RoleExistsAsync(createDto.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(createDto.Role));
                }
                if (!string.IsNullOrWhiteSpace(createDto.Role))
                    await _userManager.AddToRoleAsync(user, createDto.Role);

                taskResult.Succeeded = true;
                return taskResult;
            }

            foreach (var error in result.Errors)
            {
                _myLogger.Fatal("UserService/CreateUser", $"Entity:{JsonSerializer.Serialize(createDto)} Identity Error:{error}");
                taskResult.AddError(error.Description);
            }
            return taskResult;
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/CreateUser");
            taskResult.AddError("خطایی رخ داد!");
            taskResult.AddError("لطفا بعدا دوباره امتحان کنید!");
            return taskResult;
        }
    }

    public async Task<TaskResult> UpdateUser(UserEditDto editDto)
    {
        var taskResult = new TaskResult();
        try
        {
            Guard.Against.Null(editDto);

            UserEditDtoValidation validator = new();
            var validation = validator.Validate(editDto);
            if (validation.IsValid is false)
            {
                foreach (var failure in validation.Errors)
                    taskResult.AddError(failure.ErrorMessage);
                return taskResult;
            }

            var user = await GetUserById(editDto.Id);
            if (user is null)
            {
                taskResult.AddError("کاربری با این مشخصات موجود نیست");
                return taskResult;
            }

            if (string.IsNullOrWhiteSpace(editDto.NationalCode))
            {
                taskResult.AddError("کد ملی نمی تواند خالی باشد");
                return taskResult;
            }

            var checkIfRedunduntUsername = await GetUserByUsername(editDto.NationalCode);
            if (checkIfRedunduntUsername is not null && checkIfRedunduntUsername.Id != user.Id)
            {
                taskResult.AddError("این نام کاربری در حال حاضر به کاربر دیگری اختصاص یافته است");
                return taskResult;
            }

            var result = await _userManager.SetUserNameAsync(user, editDto.NationalCode);
            if (result.Succeeded == false)
            {
                taskResult.AddError("در فرآیند تغییر نام کاربری کاربر خطایی رخ داد");
                return taskResult;
            }

            user.IsActive = editDto.IsActive;
            user.Name = editDto.Name;
            user.LastName = editDto.LastName;
            user.NationalCode = editDto.NationalCode;

            var userRoles = await _userManager.GetRolesAsync(user);
            if (editDto.Role != userRoles.FirstOrDefault())
            {
                if (userRoles is not null && userRoles.Count > 0)
                    await _userManager.RemoveFromRoleAsync(user, userRoles.First());

                if (!string.IsNullOrWhiteSpace(editDto.Role) && !await _roleManager.RoleExistsAsync(editDto.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(editDto.Role));
                }
                if (!string.IsNullOrWhiteSpace(editDto.Role))
                    await _userManager.AddToRoleAsync(user, editDto.Role);
            }

            await _userManager.UpdateAsync(user);
            taskResult.Succeeded = true;
            return taskResult;
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/UpdateUser");
            taskResult.AddError("خطایی رخ داد!");
            taskResult.AddError("لطفا بعدا دوباره امتحان کنید!");
            return taskResult;
        }
    }

    public async Task<IReadOnlyList<UserInfo>?> GetAllUsers(string? searchName = null, string? searchNumber = null)
    {
        try
        {
            return await GetAllReadOnlyAsync(a =>
                !a.IsDeleted &&
                (string.IsNullOrEmpty(searchName) || a.Name.Contains(searchName) || a.LastName.Contains(searchName)) &&
                (string.IsNullOrEmpty(searchNumber) || a.UserName.Contains(searchNumber) || a.PhoneNumber.Contains(searchNumber)));
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/GetAllUsers");
            return null;
        }
    }

    public async Task<TaskResult> ChangeUsersPassword(AdminChangePasswordDto changePasswordDto)
    {
        var taskResult = new TaskResult();
        Guard.Against.Null(changePasswordDto);

        if (string.IsNullOrEmpty(changePasswordDto.ConfirmNewPassword) || string.IsNullOrEmpty(changePasswordDto.NewPassword))
        {
            taskResult.AddError("لطفا اطلاعات خواسته شده را وارد کنید");
            return taskResult;
        }

        changePasswordDto.NewPassword = changePasswordDto.NewPassword.Trim();
        changePasswordDto.ConfirmNewPassword = changePasswordDto.ConfirmNewPassword.Trim();

        var user = await GetUserById(changePasswordDto.Id);
        if (user is null)
        {
            taskResult.AddError("شناسه نامعتبر");
            taskResult.AddError("کاربر یافت نشد");
            return taskResult;
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, changePasswordDto.NewPassword);
        if (result.Succeeded is false)
        {
            taskResult.AddError("عوض کردن رمز عبور با شکست مواجه شد");
            foreach (var error in result.Errors)
                taskResult.AddError(error.Description);
            return taskResult;
        }

        taskResult.Succeeded = true;
        return taskResult;
    }

    public async Task<TaskResult> ChangeUsersPassword(UserChangePasswordDto changePasswordDto)
    {
        var taskResult = new TaskResult();
        Guard.Against.Null(changePasswordDto);

        if (string.IsNullOrEmpty(changePasswordDto.ConfirmNewPassword) || string.IsNullOrEmpty(changePasswordDto.NewPassword) || string.IsNullOrEmpty(changePasswordDto.Password))
        {
            taskResult.AddError("لطفا اطلاعات خواسته شده را وارد کنید");
            return taskResult;
        }

        changePasswordDto.NewPassword = changePasswordDto.NewPassword.Trim();
        changePasswordDto.ConfirmNewPassword = changePasswordDto.ConfirmNewPassword.Trim();
        changePasswordDto.Password = changePasswordDto.Password.Trim();

        var user = await GetUserById(changePasswordDto.Id);
        if (user is null)
        {
            taskResult.AddError("شناسه نامعتبر");
            taskResult.AddError("کاربر یافت نشد");
            return taskResult;
        }

        bool validUserPassword = await _userManager.CheckPasswordAsync(user, changePasswordDto.Password);
        if (validUserPassword is false)
        {
            taskResult.AddError("پسورد قبلی وارد شده معتبر نمی باشد");
            return taskResult;
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, changePasswordDto.NewPassword);
        if (result.Succeeded is false)
        {
            taskResult.AddError("عوض کردن رمز عبور با شکست مواجه شد");
            foreach (var error in result.Errors)
                taskResult.AddError(error.Description);
            return taskResult;
        }

        taskResult.Succeeded = true;
        return taskResult;
    }
}
