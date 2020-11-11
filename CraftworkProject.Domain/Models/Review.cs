using System;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Domain.Models
{
    public class Review : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Text { get; set; }
        
        [Required]
        public int Rating { get; set; }
        
        [Required]
        public DateTime PublicationDate { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }
    }
}