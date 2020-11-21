using System;
using System.Linq;
using System.Security.Claims;
using CraftworkProject.Domain;
using CraftworkProject.Infrastructure.Models;
using CraftworkProject.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CraftworkProject.Services.Implementations
{
    public class UserManagerHelper : IUserManagerHelper
    {
        private readonly UserManager<EFUser> _userManager;

        public UserManagerHelper(UserManager<EFUser> userManager)
        {
            _userManager = userManager;
        }
        
        public Guid GetUserId(ClaimsPrincipal user)
        {
            var efUser = _userManager.Users.FirstOrDefault(x => x.UserName == user.Identity.Name);

            return efUser?.Id ?? default;
        }
    }
}