﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Domain.Models
{
    public class PurchaseDetail : BaseEntity
    {
        [Required]
        public int Amount { get; set; }

        public Guid OrderId { get; set; }
        public Product Product { get; set; }
    }
}