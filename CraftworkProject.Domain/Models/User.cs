using System;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Domain.Models
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public bool EmailConfirmed { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public bool PhoneNumberConfirmed { get; set; }
    }
}