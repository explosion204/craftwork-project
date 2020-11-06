using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Domain.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Desc { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public bool InStock { get; set; }
        
        [Required]
        public string ImagePath { get; set; }

        public Category Category { get; set; }
    }
}