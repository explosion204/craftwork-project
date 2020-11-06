using System;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Web.Areas.Admin.ViewModels
{
    public class PurchaseDetailViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
    }
}