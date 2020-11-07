using System.ComponentModel.DataAnnotations;

namespace Craftwork_Project.ViewModels
{
    public class SignUpViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Passwords must be at least 8 characters.")]
        public string Password { get; set; }
        
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ConfirmEmail { get; set; }
    }
}