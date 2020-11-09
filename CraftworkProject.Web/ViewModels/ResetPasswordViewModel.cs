using System;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string Token { get; set; }
        
        [Required]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Passwords must be at least 8 characters.")]
        public string NewPassword { get; set; }
        
        [Required]
        [Display(Name = "Confirm new password")]
        [DataType(DataType.Password)]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Passwords must be at least 8 characters.")]
        public string ConfirmNewPassword { get; set; }
    }
}