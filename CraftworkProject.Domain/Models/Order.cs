using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CraftworkProject.Domain.Identity;

namespace CraftworkProject.Domain
{
    public class Order : BaseEntity
    {
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