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
        Task<User> FindUser(Guid id);
        Task<User> FindUser(string username);
        Guid GetUserId(ClaimsPrincipal user);
        Task UpdateUser(User user);
        Task DeleteUser(Guid id);
        Task DeleteUser(string username);
        Task SetUserRole(User user, Guid roleId);
        Task SetUserPassword(User user, string newPassword);
        Task<bool> SignIn(string username, string password);
        Task<bool> ConfirmEmail(Guid userId, string token);
        Task SignOut();
        Task<string> GenerateEmailConfirmationToken(User user);
    }
}