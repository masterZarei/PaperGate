using PaperGate.Core.DTOs;
using PaperGate.Core.Entities;
using PaperGate.Core.Interfaces.Repositories;
using PaperGate.Shared.ReturnTypes;

namespace PaperGate.Core.Interfaces.Services;
public interface IUserService : IGenericRepository<UserInfo>
{
    /// <summary>
    /// Gets Users information and validates it for logging in
    /// </summary>
    /// <param name="login"></param>
    /// <returns>a TaskResult with property of Succeed indicating operation was successfull or not</returns>
    Task<TaskResult> LoginUser(LoginDto login);
    /// <summary>
    /// Gets needed information and Registers the user
    /// </summary>
    /// <param name="register"></param>
    /// <returns>a TaskResult with property of Succeed indicating operation was successfull or not</returns>
    Task<TaskResult> RegisterUser(RegisterDto register);
    /// <summary>
    /// Gets users phone number and returns All users data in Result property of TaskResult
    /// </summary>
    /// <returns>All users in Result property of TaskResult</returns>
    Task<IReadOnlyList<UserInfo>?> GetAllUsers();
    /// <summary>
    /// Gets users phone number and returns All users data in Result property of TaskResult
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns>All users data in Result property of TaskResult</returns>
    Task<UserInfo?> GetUserByUsername(string phoneNumber);
    /// <summary>
    /// Gets users Id and returns All users data in Result property of TaskResult
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>All users data in Result property of TaskResult</returns>
    Task<UserInfo?> GetUserById(string userId);
    /// <summary>
    /// Gets new information of an already existing user and updates it
    /// </summary>
    /// <param name="editDto"></param>
    /// <returns>a TaskResult with property of Succeed indicating operation was successfull or not</returns>
    Task<TaskResult> UpdateUser(User_ED_Dto editDto);
    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="createrDto"></param>
    /// <returns>a TaskResult with property of Succeed indicating operation was successfull or not</returns>
    Task<TaskResult> CreateUser(UserCreateDto createrDto);
    /// <summary>
    /// Soft Removes an existing user which means it doesns actually delete the record physically
    /// </summary>
    /// <param name="deleteDto"></param>
    /// <returns>a TaskResult with property of Succeed indicating operation was successfull or not</returns>
    Task<TaskResult> SoftRemoveUser(UserDeleteDto deleteDto);
    /// <summary>
    /// Changes Users Password
    /// </summary>
    /// <param name="changePasswordDto"></param>
    /// <returns></returns>
    Task<TaskResult> ChangeUsersPassword(AdminChangePasswordDto changePasswordDto);
    Task<TaskResult> ChangeUsersPassword(UserChangePasswordDto changePasswordDto);
    /// <summary>
    /// Gets users Id and finds its roles
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>All users roles in Result property of TaskResult</returns>
    Task<string?> GetUsersRole(string userId);
    /// <summary>
    /// Returns All Roles Available
    /// </summary>
    /// <returns> All Roles Available</returns>
    Task<IQueryable?> GetRoles();
    /// <summary>
    /// Reyurns header component data such as users name and shopping cart items count
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
  //  Task<HeaderComponentDto?> GetHeaderComponentData(string userName);

}