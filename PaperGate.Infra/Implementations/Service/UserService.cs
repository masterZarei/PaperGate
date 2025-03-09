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
    protected TaskResult _taskResult;
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
        _taskResult = new();
    }



    public async Task<IQueryable?> GetRoles()
    {
        try
        {
            return _roleManager.Roles;
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/GetRoles");
            return null;
        }
    }
    public async Task<UserInfo?> GetUserById(string userId)
    {
        try
        {
            //Validation logic
            #region Validation
            Guard.Against.NullOrEmpty(userId);
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            #endregion
            UserInfo user = await GetAsync(a => a.Id == userId && !a.IsDeleted);
            if (user is null)
                return null;

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
            //Validation logic
            #region Validation
            Guard.Against.NullOrEmpty(userName);

            if (string.IsNullOrWhiteSpace(userName))
                return null;

            #endregion
            UserInfo user = await GetAsync(a => a.UserName == userName && !a.IsDeleted);
            if (user is null)
                return null;

            return user;

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

            //Validation logic
            #region Validation
            Guard.Against.NullOrEmpty(userId);
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            UserInfo user = await GetAsync(a => a.Id == userId && !a.IsDeleted);
            if (user is null)
                return null;
            #endregion


            return string.Join(",", await _userManager.GetRolesAsync(new UserInfo() { Id = user.Id })); ;

        }
        catch (Exception ex)
        {
            _myLogger.Warning(ex, "UserService/GetUsersRole");
            return null;
        }
    }
    public async Task<TaskResult> LoginUser(LoginDto login)
    {
        try
        {
            #region Validation
            Guard.Against.Null(login);

            LoginDtoValidation validator = new();
            var validation = validator.Validate(login);
            if (validation.IsValid is false)
            {
                foreach (var failure in validation.Errors)
                {
                    _taskResult.AddError(failure.ErrorMessage);
                }
                return _taskResult;
            }
            #endregion

            //Check if user exists in the Datebase
            UserInfo User = await GetAsync(a => a.NationalCode == login.NationalCode);
            if (User is null || User.IsDeleted)
            {
                _taskResult.AddError("کاربری با این مشخصات موجود نیست");
                return _taskResult;
            }
            //Check if Signing user is valid or not
            var result = await _signInManager.PasswordSignInAsync(login.NationalCode, login.Password, true, false);
            if (result.Succeeded)
            {
                _taskResult.Result = User;
                _taskResult.Succeeded = true;
                return _taskResult;
            }
            else if (result.IsLockedOut)
            {
                _taskResult.AddError($"کاربر {User.NationalCode} قفل شده است!");
                return _taskResult;
            }
            _taskResult.AddError("شماره همراه یا رمز عبور نامعتبر است");
            return _taskResult;
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/LoginUser");
            _taskResult.AddError("خطایی رخ داد!");
            _taskResult.AddError("لطفا بعدا دوباره امتحان کنید!");
            return _taskResult;
        }
    }
    public async Task<TaskResult> RegisterUser(RegisterDto register)
    {
        try
        {
            #region Validation
            Guard.Against.Null(register);

            RegisterDtoValidation validator = new();
            var validation = validator.Validate(register);
            if (validation.IsValid is false)
            {
                foreach (var failure in validation.Errors)
                {
                    _taskResult.AddError(failure.ErrorMessage);
                }
                return _taskResult;
            }
            #endregion
            //Registering the User
            var user = Activator.CreateInstance<UserInfo>();

            user.Name = register.Name;
            user.LastName = register.LastName;
            user.NationalCode = register.NationalCode;

            await _userStore.SetUserNameAsync(user, register.NationalCode, CancellationToken.None);

            /*            user.ActivationCode = CodeGenerator.GenerateUserActivationCode;
            */
            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                await AssignRole(user);

                await _signInManager.SignInAsync(user, isPersistent: false);
                _taskResult.Succeeded = true;
                return _taskResult;
            }
            foreach (var error in result.Errors)
            {
                _taskResult.AddError(error.Description);
            }
            // If we got this far, something failed
            return _taskResult;
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/RegisterUser");
            _taskResult.AddError("خطایی رخ داد!");
            _taskResult.AddError("لطفا بعدا دوباره امتحان کنید!");
            return _taskResult;
        }
    }
    private async Task AssignRole(UserInfo user)
    {
        Guard.Against.Null(user);
        if (!await _roleManager.RoleExistsAsync(Roles.AdminEndUser))
            await _roleManager.CreateAsync(new IdentityRole(Roles.AdminEndUser));

        if (!await _roleManager.RoleExistsAsync(Roles.StudentEndUser))
            await _roleManager.CreateAsync(new IdentityRole(Roles.StudentEndUser));

        if (_db.Users.ToList().Count == 1)
            await _userManager.AddToRoleAsync(user, Roles.AdminEndUser);

        else
            await _userManager.AddToRoleAsync(user, Roles.StudentEndUser);
    }
    public async Task<TaskResult> SoftRemoveUser(UserDeleteDto deleteDto)
    {
        try
        {
            #region Validation
            Guard.Against.Null(deleteDto);
            if (string.IsNullOrWhiteSpace(deleteDto.Id))
            {
                _taskResult.AddError("شناسه کاربر نمی تواند خالی باشد.");
                return _taskResult;
            }
            UserInfo? user = await GetUserById(deleteDto.Id);
            if (user is null)
            {
                _taskResult.AddError("کاربری با این مشخصات موجود نیست");
                return _taskResult;
            }
            #endregion

            user.IsDeleted = true;

            await UpdateAsync(user);
            await _db.SaveChangesAsync();
            _taskResult.Succeeded = true;
            return _taskResult;
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/SoftRemoveUser");
            _taskResult.AddError("خطایی رخ داد!");
            _taskResult.AddError("لطفا بعدا دوباره امتحان کنید!");
            return _taskResult;
        }
    }
    public async Task<TaskResult> CreateUser(UserCreateDto createDto)
    {
        try
        {//NationalCode doesnt get saved
            #region Validation
            Guard.Against.Null(createDto);

            UserCreateDtoValidation validator = new();
            var validation = validator.Validate(createDto);
            if (validation.IsValid is false)
            {
                foreach (var failure in validation.Errors)
                {
                    _taskResult.AddError(failure.ErrorMessage);
                }
                return _taskResult;
            }
            #endregion
            var user = Activator.CreateInstance<UserInfo>();
            user = _mapper.Map<UserInfo>(createDto);
            await _userStore.SetUserNameAsync(user, createDto.NationalCode, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, createDto.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(createDto.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(createDto.Role));
                }
                await _userManager.AddToRoleAsync(user, createDto.Role);

                _taskResult.Succeeded = true;
                return _taskResult;
            }
            foreach (var error in result.Errors)
            {
                _myLogger.Fatal("UserService/CreateUser", $"Entity:{JsonSerializer.Serialize(createDto)} Identity Error:{error}");
                _taskResult.AddError(error.Description);
            }
            return _taskResult;
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/CreateUser");
            _taskResult.AddError("خطایی رخ داد!");
            _taskResult.AddError("لطفا بعدا دوباره امتحان کنید!");
            return _taskResult;
        }

    }
    public async Task<TaskResult> UpdateUser(User_ED_Dto editDto)
    {
        try
        {
            #region Validation
            Guard.Against.Null(editDto);

            UserEditDtoValidation validator = new();
            var validation = validator.Validate(editDto);
            if (validation.IsValid is false)
            {
                foreach (var failure in validation.Errors)
                {
                    _taskResult.AddError(failure.ErrorMessage);
                }
                return _taskResult;
            }
            #endregion
            var user = await GetUserById(editDto.Id);
            if (user is null)
            {
                _taskResult.AddError("کاربری با این مشخصات موجود نیست");
                return _taskResult;
            }
            if (user is null)
            {
                _taskResult.AddError("کاربری با این مشخصات موجود نیست");
                return _taskResult;
            }
            var checkIfRedunduntUsername = await GetUserByUsername(editDto.NationalCode);
            if (checkIfRedunduntUsername is not null && checkIfRedunduntUsername != user)
            {
                _taskResult.AddError("این نام کاربری در حال حاضر به کاربر دیگری اختصاص یافته است");
                return _taskResult;
            }
            //We cant use AutoMapper because Id gets changed too
            // user = _mapper.Map<UserInfo>(editDto);
            var result = await _userManager.SetUserNameAsync(user, editDto.NationalCode);
            if (result.Succeeded == false)
            {
                _taskResult.AddError("در فرآیند تغییر نام کاربری کاربر خطایی رخ داد");
                return _taskResult;
            }
            user.IsActive = editDto.IsActive;
            user.Name = editDto.Name;
            user.LastName = editDto.LastName;
            user.NationalCode = editDto.NationalCode;
            user.NormalizedUserName = editDto.NationalCode;
            /*user.CartNumber = editDto.CartNumber;
            user.ActivationCode = CodeGenerator.GenerateUserActivationCode;
            user.Email = editDto.Email;*/


            user.NationalCode = editDto.NationalCode;
            //نقش کاربرو بگیر
            var userRoles = await _userManager.GetRolesAsync(new UserInfo() { Id = user.Id });

            //اگه عوض شده بود
            if (editDto.Role != userRoles.FirstOrDefault())
            {
                //نقش قبلیش رو پاک کن
                if (userRoles is not null && userRoles.Count > 0)
                    await _userManager.RemoveFromRoleAsync(new UserInfo() { Id = user.Id }, userRoles.First());
                //نقش انتخاب شده رو بهش بده
                //اول مطمئن شو نقش وجود داره و معتبره
                if (!await _roleManager.RoleExistsAsync(editDto.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(editDto.Role));
                }
                await _userManager.AddToRoleAsync(new UserInfo() { Id = user.Id }, editDto.Role);
            }
            await _userManager.UpdateAsync(user);
            _taskResult.Succeeded = true;
            return _taskResult;
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/UpdateUser");
            _taskResult.AddError("خطایی رخ داد!");
            _taskResult.AddError("لطفا بعدا دوباره امتحان کنید!");
            return _taskResult;
        }

    }
    public async Task<IReadOnlyList<UserInfo>?> GetAllUsers()
    {
        try
        {
            return await GetAllAsync(a => !a.IsDeleted);
        }
        catch (Exception ex)
        {
            _myLogger.Fatal(ex, "UserService/GetAllUsers");
            return null;
        }
    }
    public async Task<TaskResult> ChangeUsersPassword(AdminChangePasswordDto changePasswordDto)
    {
        #region Validation
        Guard.Against.Null(changePasswordDto);
        if (string.IsNullOrEmpty(changePasswordDto.ConfirmNewPassword) || string.IsNullOrEmpty(changePasswordDto.NewPassword))
        {
            _taskResult.AddError("لطفا اطلاعات خواسته شده را وارد کنید");
            return _taskResult;
        }


        changePasswordDto.NewPassword = changePasswordDto.NewPassword.Trim();
        changePasswordDto.ConfirmNewPassword = changePasswordDto.ConfirmNewPassword.Trim();
        #endregion
        var user = await GetUserById(changePasswordDto.Id);
        if (user is null)
        {
            _taskResult.AddError("شناسه نامعتبر");
            _taskResult.AddError("کاربر یافت نشد");
            return _taskResult;
        }
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, changePasswordDto.NewPassword);
        if (result.Succeeded is false)
        {
            _taskResult.AddError("عوض کردن رمز عبور با شکست مواجه شد");
            foreach (var error in result.Errors)
            {
                _taskResult.AddError(error.Description);
            }
            return _taskResult;
        }

        _taskResult.Succeeded = true;
        return _taskResult;
    }
    public async Task<TaskResult> ChangeUsersPassword(UserChangePasswordDto changePasswordDto)
    {
        #region Validation
        Guard.Against.Null(changePasswordDto);

        if (string.IsNullOrEmpty(changePasswordDto.ConfirmNewPassword) || string.IsNullOrEmpty(changePasswordDto.NewPassword) || string.IsNullOrEmpty(changePasswordDto.Password))
        {
            _taskResult.AddError("لطفا اطلاعات خواسته شده را وارد کنید");
            return _taskResult;
        }

        changePasswordDto.NewPassword = changePasswordDto.NewPassword.Trim();
        changePasswordDto.ConfirmNewPassword = changePasswordDto.ConfirmNewPassword.Trim();
        changePasswordDto.Password = changePasswordDto.Password.Trim();

        #endregion
        var user = await GetUserById(changePasswordDto.Id);
        if (user is null)
        {
            _taskResult.AddError("شناسه نامعتبر");
            _taskResult.AddError("کاربر یافت نشد");
            return _taskResult;
        }


        if (string.IsNullOrEmpty(changePasswordDto.Password))
        {
            _taskResult.AddError("پسورد قبلی نمی تواند خالی وارد شود");
            return _taskResult;
        }
        bool validUserPassword = await _userManager.CheckPasswordAsync(user, changePasswordDto.Password);
        if (validUserPassword is false)
        {
            _taskResult.AddError("پسورد قبلی وارد شده معتبر نمی باشد");
            return _taskResult;
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, changePasswordDto.NewPassword);
        if (result.Succeeded is false)
        {
            _taskResult.AddError("عوض کردن رمز عبور با شکست مواجه شد");
            foreach (var error in result.Errors)
            {
                _taskResult.AddError(error.Description);
            }
            return _taskResult;
        }

        _taskResult.Succeeded = true;
        return _taskResult;
    }

    /*    public async Task<HeaderComponentDto?> GetHeaderComponentData(string userName)
        {
            try
            {
                #region Validation
                Guard.Against.NullOrEmpty(userName);

                //Chack is user is valid
                var user = await GetUserByUsername(userName);
                if (user is null)
                {
                    await _myLogger.Fatal("UserService/GetHeaderComponentData", "Username is not Valid. User wasn't found");
                    return null;
                }
                #endregion

                var userFactor = await _unitOfWork.Factor.GetAsync(f => f.UserId == user.Id, includeProperties: "FactorDetails");
                HeaderComponentDto componentDto = new()
                {
                    Name = $"{user.Name} {user.LastName}",
                    ProductsInShoppingcartCount = userFactor is null ? 0 : userFactor.FactorDetails.Count
                };
                return componentDto;

            }
            catch (Exception ex)
            {
                _myLogger.Fatal(ex, "UserService/GetHeaderComponentData");
                return null;
            }

        }*/
}
