using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CraftworkProject.Domain.Models;

namespace CraftworkProject.Domain
{
    public interface IUserManager
    {
        List<User> GetAllUsers();
        List<UserRole> GetAllRoles();
        Task<Guid> GetRoleId(string roleName);
        Task<Guid> GetUserRoleId(Guid userId);
        Task<bool> CreateUser(User newUser, string password, Guid roleId);
        Task<User> FindUserById(Guid id);
        Task<User> FindUserByName(string username);
        Task<User> FindUserByEmail(string email);
        Guid GetUserId(ClaimsPrincipal user);
        Task UpdateUser(User user);
        Task DeleteUser(Guid id);
        Task DeleteUser(string username);
        Task SetUserRole(Guid userId, Guid roleId);
        Task SetUserPassword(Guid userId, string newPassword);
        Task<bool> ChangeUserPassword(Guid userId, string currentPassword, string newPassword);
        Task<bool> SignIn(string username, string password);
        Task<bool> ConfirmEmail(Guid userId, string token);
        Task<bool> ResetPassword(Guid userId, string token, string newPassword);
        Task SignOut();
        Task<string> GenerateEmailConfirmationToken(Guid userId);
        Task<string> GeneratePasswordResetToken(Guid userId);
    }
}