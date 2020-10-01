using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Craftwork_Project.Domain.Models
{
    public class PurchaseDetail
    {
        [Required]
        public Guid Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        
        public Order Order { get; set; }
        
        [Required]
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        
        public Product Product { get; set; }
        
        [Required]
        public int Amount { get; set; }

    }
}