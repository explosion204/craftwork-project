using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}