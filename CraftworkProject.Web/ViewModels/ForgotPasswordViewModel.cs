using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Web.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}