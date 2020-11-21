using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CraftworkProject.Infrastructure.Models;
using CraftworkProject.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace CraftworkProject.Services.Implementations
{
    public class UserManagerHelper : IUserManagerHelper
    {
        private readonly UserManager<EFUser> _userManager;
        private readonly SignInManager<EFUser> _signInManager;

        public UserManagerHelper(
            UserManager<EFUser> userManager, 
            SignInManager<EFUser> signInManager
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        public Guid GetUserId(ClaimsPrincipal user)
        {
            var efUser = _userManager.Users.FirstOrDefault(x => x.UserName == user.Identity.Name);

            return efUser?.Id ?? default;
        }

        public async Task<List<AuthenticationScheme>> GetExternalAuthenticationSchemes()
        {
            return (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string providerName, string callbackUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(providerName, callbackUrl);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfo()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }

        public async Task AddExternalLogin(Guid userId, ExternalLoginInfo info)
        {
            var efUser = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.AddLoginAsync(efUser, info);
        }

        public async Task<bool> ExternalLoginSignIn(ExternalLoginInfo info)
        {
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                false, true);

            return result.Succeeded;
        }
    }
}