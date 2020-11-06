using System;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Domain.Models
{
    public class UserRole
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}