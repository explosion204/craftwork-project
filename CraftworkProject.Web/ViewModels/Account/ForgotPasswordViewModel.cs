using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Web.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}