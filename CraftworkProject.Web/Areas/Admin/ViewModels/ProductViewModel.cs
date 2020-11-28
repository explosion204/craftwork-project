using System;
using System.ComponentModel.DataAnnotations;
using CraftworkProject.Web.Service.Validation;
using Microsoft.AspNetCore.Http;

namespace CraftworkProject.Web.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string ShortDesc { get; set; }
        
        [Required]
        public string Desc { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public bool InStock { get; set; }
        
        [Display(Name = "Product image")]
        [AllowedExtensions(new [] { ".jpg", ".jpeg", ".png" })]
        public IFormFile Image { get; set; }
        
        public string ImagePath { get; set; }
        
        public double Rating { get; set; }
        public int RatesCount { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}