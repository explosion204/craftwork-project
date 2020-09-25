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
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [Required]
        public IdentityUser<Guid> User { get; set; }
        [Required]
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        [Required]
        public Product Product { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public bool InCart { get; set; }
        [Required]
        public bool Confirmed { get; set; }
        [Required]
        public bool Processed { get; set; }
    }
}