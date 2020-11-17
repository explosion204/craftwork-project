using System;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Web.Areas.Admin.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        public DateTime Created { get; set; }
        
        [Required]
        public bool Processed { get; set; }
        
        [Required]
        public bool Canceled { get; set; }
        
        [Required]
        public bool Finished { get; set; }
    }
}