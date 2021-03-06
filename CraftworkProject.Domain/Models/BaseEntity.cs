using System;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Domain.Models
{
    public abstract class BaseEntity
    {
        [Required]
        public Guid Id { get; set; }
    }
}