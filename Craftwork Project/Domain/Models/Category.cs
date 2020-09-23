using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Craftwork_Project.Domain.Models
{
    public class Category
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }
        
        public List<Product> Products { get; set; }
    }
}