using System;
using System.ComponentModel.DataAnnotations;
using CraftworkProject.Web.Service.Validation;
using Microsoft.AspNetCore.Http;

namespace CraftworkProject.Web.Areas.Admin.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        
        [Required]
        [Display(Name = "Role")]
        public Guid RoleId { get; set; }

        [Display(Name = "New password")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Passwords must be at least 8 characters.")]
        public string NewPassword { get; set; }
        
        [Display(Name = "Confirm new password")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Passwords must be at least 8 characters.")]
        public string ConfirmNewPassword { get; set; }
        
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        
        [Display(Name = "Verified")]
        public bool Verified { get; set; }
        
        [Display(Name = "Phone number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        
        [Required]
        [Display(Name = "Phone number verified")]
        public bool PhoneNumberConfirmed { get; set; }
        
        [Display(Name = "Profile image")]
        [AllowedExtensions(new [] { ".jpg", ".jpeg", ".bmp" })]
        [MaxFileSize(1024)] // kbytes 
        public IFormFile ProfilePicture { get; set; }
    }
}