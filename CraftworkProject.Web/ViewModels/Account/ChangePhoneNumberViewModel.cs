using System;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Web.ViewModels.Account
{
    public class ChangePhoneNumberViewModel
    {
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        
        public string Code { get; set; }
    }
}