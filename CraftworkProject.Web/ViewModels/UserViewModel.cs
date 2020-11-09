using System;
using System.ComponentModel.DataAnnotations;
using CraftworkProject.Web.Service.Validation;
using Microsoft.AspNetCore.Http;

namespace CraftworkProject.Web.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public Guid Id { get; set; }
        
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
        [Display(Name = "Current password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
         
        [Required]
        [Display(Name = "New password")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Passwords must be at least 8 characters.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [Required]
        [Display(Name = "Confirm new password")]
        [DataType(DataType.Password)]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Passwords must be at least 8 characters.")]
        public string ConfirmNewPassword { get; set; }
                
        // [Display(Name = "Email")]
        // [EmailAddress(ErrorMessage = "Invalid email address")]
        // public string Email { get; set; }
        //         
        // [Display(Name = "Verified")]
        // public bool Verified { get; set; }
                
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        //         
        // [Required]
        // [Display(Name = "Phone number verified")]
        // public bool PhoneNumberConfirmed { get; set; }
                
        [Display(Name = "Profile image")]
        [AllowedExtensions(new string [] { ".jpg", ".jpeg", ".bmp" })]
        [MaxFileSize(1024)] // kbytes 
        public IFormFile ProfilePicture { get; set; }
        
        public string CurrentProfilePicture { get; set; }
    }
}