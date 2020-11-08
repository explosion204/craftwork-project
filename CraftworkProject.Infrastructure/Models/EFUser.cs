using System;
using Microsoft.AspNetCore.Identity;

namespace CraftworkProject.Infrastructure.Models
{
    public class EFUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
    }
}