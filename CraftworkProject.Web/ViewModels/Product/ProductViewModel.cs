using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CraftworkProject.Domain.Models;

namespace CraftworkProject.Web.ViewModels.Product
{
    public class ProductViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public string ImagePath { get; set; }
        public double Rating { get; set; }
        public int RatesCount { get; set; }
        public Domain.Models.Category Category { get; set; }
        public List<Review> Reviews { get; set; }
        public bool CurrentUserReviewExists { get; set; }
        
        public Guid ReviewId { get; set; }
        
        [Required]
        public string ReviewTitle { get; set; }
        
        [Required]
        public string ReviewText { get; set; }
        
        public int ReviewRating { get; set; }
    }
}