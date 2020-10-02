using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftworkProject.Domain
{
    public class PurchaseDetail : BaseEntity
    {
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        
        public Order Order { get; set; }
        
        [Required]
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        
        public Product Product { get; set; }
        
        [Required]
        public int Amount { get; set; }

    }
}