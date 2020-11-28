using System;
using Microsoft.AspNetCore.Identity;

namespace CraftworkProject.Infrastructure
{
    internal class EFUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
    }
}