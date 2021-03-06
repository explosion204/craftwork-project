using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CraftworkProject.Infrastructure
{
    internal class ApplicationUserManager : IUserManager
    {
        private readonly UserManager<EFUser> _userManager;
        private readonly RoleManager<EFUserRole> _roleManager;
        private readonly SignInManager<EFUser> _signInManager;
        private readonly IRepository<Review> _reviewRepository;
        private readonly IMapper _mapper;

        public ApplicationUserManager(
            UserManager<EFUser> userManager, 
            RoleManager<EFUserRole> roleManager, 
            SignInManager<EFUser> signInManager, 
            IRepository<Review> reviewRepository,
            IMapper mapper
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }
        
        public List<User> GetAllUsers()
        {
            var efUsers = _userManager.Users.ToList();
            return _mapper.Map<List<User>>(efUsers);
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

        public async Task<Guid> CreateUser(User newUser, string password, Guid roleId)
        {
            var efUser = new EFUser()
            {
                Id = newUser.Id,
                UserName = newUser.Username,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                EmailConfirmed = newUser.EmailConfirmed,
                PhoneNumber = newUser.PhoneNumber,
                PhoneNumberConfirmed = newUser.PhoneNumberConfirmed,
                ProfilePicture = newUser.ProfilePicture
            };
            var userCreation = await _userManager.CreateAsync(efUser, password);

            if (userCreation.Succeeded)
            {
                var userRole = await _roleManager.FindByIdAsync(roleId.ToString());
                var roleAssigning = await _userManager.AddToRoleAsync(efUser, userRole.Name);
                if (roleAssigning.Succeeded)
                {
                    return efUser.Id;
                }
            }

            return default;
        }

        public async Task<User> FindUserById(Guid id)
        {
            var efUser = await _userManager.FindByIdAsync(id.ToString());

            return _mapper.Map<User>(efUser);
        }

        public async Task<User> FindUserByName(string username)
        {
            var efUser = await _userManager.FindByNameAsync(username);

            return efUser != null ? _mapper.Map<User>(efUser) : null;
        }

        public async Task<User> FindUserByEmail(string email)
        {
            var efUser = await _userManager.FindByEmailAsync(email);

            return efUser != null ? _mapper.Map<User>(efUser) : null;
        }

        public async Task<User> FindUserByPhoneNumber(string phoneNumber)
        {
            var efUser = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

            return efUser != null ? _mapper.Map<User>(efUser) : null;
        }

        public async Task UpdateUser(User user)
        {
            var efUser = await _userManager.FindByIdAsync(user.Id.ToString());
            efUser.FirstName = user.FirstName;
            efUser.LastName = user.LastName;
            efUser.Email = user.Email;
            efUser.EmailConfirmed = user.EmailConfirmed;
            efUser.PhoneNumber = user.PhoneNumber;
            efUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            efUser.ProfilePicture = user.ProfilePicture;
            await _userManager.UpdateAsync(efUser);
        }

        public async Task DeleteUser(Guid id)
        {
            var efUser = await _userManager.FindByIdAsync(id.ToString());
            
            if (efUser != null)
            {
                await _userManager.DeleteAsync(efUser);
                CascadeDeleteReviews(efUser);
            }
        }

        public async Task DeleteUser(string username)
        {
            var efUser = await _userManager.FindByNameAsync(username);

            if (efUser != null)
            {
                await _userManager.DeleteAsync(efUser);
                CascadeDeleteReviews(efUser);
            }
        }

        //TODO: method is called but cascade delete is not executed
        private void CascadeDeleteReviews(EFUser efUser)
        {
            var associatedReviews = _reviewRepository.GetAllEntities().Where(x => x.User.Id == efUser.Id);

            foreach (var review in associatedReviews)
            {
                _reviewRepository.DeleteEntity(review.Id);
            }
        }

        public async Task SetUserRole(Guid userId, Guid roleId)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());
            var userRoles = await _userManager.GetRolesAsync(efUser);
            var newRole = await _roleManager.FindByIdAsync(roleId.ToString());
            await _userManager.RemoveFromRoleAsync(efUser, userRoles[0]);
            await _userManager.AddToRoleAsync(efUser, newRole.Name);
            await _userManager.UpdateAsync(efUser);
        }

        public async Task SetUserPassword(Guid userId, string newPassword)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());
            var token = await _userManager.GeneratePasswordResetTokenAsync(efUser);
            await _userManager.ResetPasswordAsync(efUser, token, newPassword);
            await _userManager.UpdateAsync(efUser);
        }

        public async Task<bool> ChangeUserPassword(Guid userId, string currentPassword, string newPassword)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.ChangePasswordAsync(efUser, currentPassword, newPassword);

            return result.Succeeded;
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

        public async Task<bool> ConfirmPhoneNumber(Guid userId, string token)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());

            if (efUser != null)
            {
                var status = await _userManager.ChangePhoneNumberAsync(efUser, efUser.PhoneNumber, token);
                return status.Succeeded;
            }

            return false;
        }

        public async Task<bool> ResetPassword(Guid userId, string token, string newPassword)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());

            if (efUser != null)
            {
                var status = await _userManager.ResetPasswordAsync(efUser, token, newPassword);
                return status.Succeeded;
            }

            return false;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GenerateEmailConfirmationToken(Guid userId)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());
            
            return await _userManager.GenerateEmailConfirmationTokenAsync(efUser);
        }

        public async Task<string> GeneratePasswordResetToken(Guid userId)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());

            return await _userManager.GeneratePasswordResetTokenAsync(efUser);
        }

        public async Task<string> GenerateChangePhoneNumberToken(Guid userId, string phoneNumber)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());

            return await _userManager.GenerateChangePhoneNumberTokenAsync(efUser, phoneNumber);
        }
    }
}