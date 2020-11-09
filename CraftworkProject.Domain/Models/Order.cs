using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Domain.Models
{
    public class Order : BaseEntity
    {
        [Required]
        public bool Processed { get; set; }
        
        [Required]
        public bool Finished { get; set; }
        
        public User User { get; set; }
        public List<PurchaseDetail> PurchaseDetails { get; set; }
    }
}