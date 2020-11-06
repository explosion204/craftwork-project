using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Domain.Models
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Desc { get; set; }
        
        public List<Product> Products { get; set; }
    }
}