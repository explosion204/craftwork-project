using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Craftwork_Project.Domain.Models
{
    public class Product
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime Created { get; set; }
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