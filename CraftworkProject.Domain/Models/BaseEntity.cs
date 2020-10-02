using System;
using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Domain
{
    public abstract class BaseEntity
    {
        [Required]
        public Guid Id { get; set; }
    }
}