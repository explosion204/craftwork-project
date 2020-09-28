using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Craftwork_Project.Domain.Models
{
    public class PurchaseDetail
    {
        public int? OrderId { get; set; }
        
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        
        public ApplicationUser User { get; set; }
        
        [Required]
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        
        public Product Product { get; set; }
        
        [Required]
        public int Amount { get; set; }
        
        [Required]
        public bool Processed { get; set; }
        
        [Required]
        public bool Finished { get; set; }
        
    }
}