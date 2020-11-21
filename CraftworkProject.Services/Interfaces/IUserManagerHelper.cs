using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace CraftworkProject.Services.Interfaces
{
    public interface IUserManagerHelper
    {
        Guid GetUserId(ClaimsPrincipal user);
        Task<List<AuthenticationScheme>> GetExternalAuthenticationSchemes();
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string providerName, string callbackUrl);
        Task<ExternalLoginInfo> GetExternalLoginInfo();
        Task AddExternalLogin(Guid userId, ExternalLoginInfo info);
        Task<bool> ExternalLoginSignIn(ExternalLoginInfo info);
    }
}