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
        Task<bool> CreateUser(User newUser, string password, Guid roleId);
        Task<User> FindUser(Guid id);
        Task<User> FindUser(string username);
        Guid GetUserId(ClaimsPrincipal user);
        void UpdateUser(User user);
        void DeleteUser(Guid id);
        void SetUserRole(User user, Guid roleId);
        void SetUserPassword(User user, string newPassword);
        Task<bool> SignIn(string username, string password);
        void SignOut();
    }
}