using System;

namespace CraftworkProject.Infrastructure.Models
{
    public class EFOrder
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public bool Processed { get; set; }
        public bool Finished { get; set; }
    }
}