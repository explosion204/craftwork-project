using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Web.ViewModels.Account
{
    public class SignUpViewModel
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