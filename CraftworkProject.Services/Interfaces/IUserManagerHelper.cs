using System;
using System.Security.Claims;

namespace CraftworkProject.Services.Interfaces
{
    public interface IUserManagerHelper
    {
        Guid GetUserId(ClaimsPrincipal user);
    }
}