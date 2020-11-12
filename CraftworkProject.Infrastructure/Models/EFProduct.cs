using System;

namespace CraftworkProject.Infrastructure.Models
{
    public class EFProduct
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public string ImagePath { get; set; }
        public double Rating { get; set; }
        public int RatesCount { get; set; }
    }
}