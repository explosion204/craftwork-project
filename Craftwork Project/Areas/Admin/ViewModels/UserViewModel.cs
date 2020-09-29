using System;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace Craftwork_Project.Areas.Admin.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
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
        
    }
}