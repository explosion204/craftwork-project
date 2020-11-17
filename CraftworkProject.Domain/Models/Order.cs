using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftworkProject.Domain.Models
{
    public class Order : BaseEntity
    {
        public DateTime Created { get; set; }
        
        [Required]
        public bool Processed { get; set; }
        
        [Required]
        public bool Canceled { get; set; }
        
        [Required]
        public bool Finished { get; set; }
        
        public User User { get; set; }
        public List<PurchaseDetail> PurchaseDetails { get; set; }
    }
}