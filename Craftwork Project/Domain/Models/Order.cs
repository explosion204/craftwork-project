using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Craftwork_Project.Domain.Models
{
    public class Order
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        
        public ApplicationUser User { get; set; }
        
        [Required]
        public bool Processed { get; set; }
        
        [Required]
        public bool Finished { get; set; }
        
        public List<Product> PurchaseDetails { get; set; }
    }
}