using System;

namespace CraftworkProject.Infrastructure
{
    internal class EFOrder
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public bool Processed { get; set; }
        public bool Canceled { get; set; }
        public bool Finished { get; set; }
    }
}