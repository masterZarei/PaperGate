using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;
using PaperGate.Shared.ReturnTypes;

namespace PaperGate.Core.Interfaces.Services;
public interface IUserService
{
    Task<TaskResult> LoginUser(LoginDto login);
    Task<TaskResult> RegisterUser(RegisterDto register);
    Task<IReadOnlyList<UserInfo>?> GetAllUsers(string? searchName = null, string? searchNumber = null);
    Task<UserInfo?> GetUserByUsername(string phoneNumber);
    Task<UserInfo?> GetUserById(string userId);
    Task<TaskResult> UpdateUser(UserEditDto editDto);
    Task<TaskResult> CreateUser(UserCreateDto createrDto);
    Task<TaskResult> SoftRemoveUser(UserDeleteDto deleteDto);
    Task<TaskResult> ChangeUsersPassword(AdminChangePasswordDto changePasswordDto);
    Task<TaskResult> ChangeUsersPassword(UserChangePasswordDto changePasswordDto);
    Task<string?> GetUsersRole(string userId);
}
