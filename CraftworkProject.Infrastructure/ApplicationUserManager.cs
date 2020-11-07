using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace CraftworkProject.Infrastructure
{
    public class ApplicationUserManager : IUserManager
    {
        private readonly UserManager<EFUser> _userManager;
        private readonly RoleManager<EFUserRole> _roleManager;
        private readonly SignInManager<EFUser> _signInManager;
        private readonly IMapper _mapper;

        public ApplicationUserManager(
            UserManager<EFUser> userManager, 
            RoleManager<EFUserRole> roleManager, 
            SignInManager<EFUser> signInManager, 
            IMapper mapper
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        
        public List<User> GetAllUsers()
        {
            return _mapper.Map<List<User>>(_userManager.Users.ToList());
        }

        public List<UserRole> GetAllRoles()
        {
            return _mapper.Map<List<UserRole>>(_roleManager.Roles.ToList());
        }

        public async Task<Guid> GetRoleId(string roleName)
        {
            var efRole = await _roleManager.FindByNameAsync(roleName);

            return efRole.Id;
        }

        public async Task<Guid> GetUserRoleId(Guid userId)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());
            var userRoles = await _userManager.GetRolesAsync(efUser);
            var roleId = await GetRoleId(userRoles[0]);
            return roleId;
        }

        public async Task<bool> CreateUser(User newUser, string password, Guid roleId)
        {
            //var efUser = _mapper.Map<EFUser>(newUser);
            var efUser = new EFUser()
            {
                Id = newUser.Id,
                UserName = newUser.Username,
                Email = newUser.Email,
                EmailConfirmed = newUser.EmailConfirmed,
                PhoneNumber = newUser.PhoneNumber,
                PhoneNumberConfirmed = newUser.PhoneNumberConfirmed
            };
            var userCreation = await _userManager.CreateAsync(efUser, password);
            
            if (userCreation.Succeeded)
            {
                var userRole = await _roleManager.FindByIdAsync(roleId.ToString());
                var roleAssigning = await _userManager.AddToRoleAsync(efUser, userRole.Name);
                if (roleAssigning.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<User> FindUser(Guid id)
        {
            var efUser = await _userManager.FindByIdAsync(id.ToString());

            return _mapper.Map<User>(efUser);
        }

        public async Task<User> FindUser(string username)
        {
            var efUser = await _userManager.FindByNameAsync(username);

            return efUser != null ? _mapper.Map<User>(efUser) : null;
        }

        public Guid GetUserId(ClaimsPrincipal user)
        {
            var efUser = _userManager.Users.FirstOrDefault(x => x.UserName == user.Identity.Name);

            return efUser.Id;
        }

        public async Task UpdateUser(User user)
        {
            //var efUser = _mapper.Map<EFUser>(user);
            var efUser = await _userManager.FindByIdAsync(user.Id.ToString());
            efUser.Email = user.Email;
            efUser.EmailConfirmed = user.EmailConfirmed;
            efUser.PhoneNumber = user.PhoneNumber;
            efUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            await _userManager.UpdateAsync(efUser);
        }

        public async Task DeleteUser(Guid id)
        {
            var efUser = await _userManager.FindByIdAsync(id.ToString());
            
            if (efUser != null)
            {
                await _userManager.DeleteAsync(efUser);
            }
        }

        public async Task DeleteUser(string username)
        {
            var efUser = await _userManager.FindByNameAsync(username);

            if (efUser != null)
            {
                await _userManager.DeleteAsync(efUser);
            }
        }

        public async Task SetUserRole(User user, Guid roleId)
        {
            var efUser = await _userManager.FindByIdAsync(user.Id.ToString());
            var userRoles = await _userManager.GetRolesAsync(efUser);
            var newRole = await _roleManager.FindByIdAsync(roleId.ToString());
            await _userManager.RemoveFromRoleAsync(efUser, userRoles[0]);
            await _userManager.AddToRoleAsync(efUser, newRole.Name);
            await _userManager.UpdateAsync(efUser);
        }

        public async Task SetUserPassword(User user, string newPassword)
        {
            var efUser = await _userManager.FindByIdAsync(user.Id.ToString());
            var token = await _userManager.GeneratePasswordResetTokenAsync(efUser);
            await _userManager.ResetPasswordAsync(efUser, token, newPassword);
            await _userManager.UpdateAsync(efUser);
        }

        public async Task<bool> SignIn(string username, string password)
        {
            var efUser = await _userManager.FindByNameAsync(username);
            
            if (efUser != null)
            {
                await _signInManager.SignOutAsync();
                SignInResult result =
                    await _signInManager.PasswordSignInAsync(efUser, password, false, false);

                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> ConfirmEmail(Guid userId, string token)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());

            if (efUser != null)
            {
                var status = await _userManager.ConfirmEmailAsync(efUser, token);
                return status.Succeeded;
            }

            return false;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GenerateEmailConfirmationToken(User user)
        {
            var efUser = await _userManager.FindByIdAsync(user.Id.ToString());
            
            return await _userManager.GenerateEmailConfirmationTokenAsync(efUser);
        }
    }
}