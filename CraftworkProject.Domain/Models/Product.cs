using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftworkProject.Domain
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
        
        [Required]
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        
        public Category Category { get; set; }
    }
}