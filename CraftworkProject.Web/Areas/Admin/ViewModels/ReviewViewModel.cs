using System;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Web.Areas.Admin.ViewModels
{
    public class ReviewViewModel
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        [Required]
        public string Text { get; set; }
        
        [Required]
        public int Rating { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid ProductId { get; set; }
        
        public Guid PublicationDate { get; set; }
    }
}