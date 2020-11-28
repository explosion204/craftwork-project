using System;

namespace CraftworkProject.Infrastructure
{
    internal class EFPurchaseDetail
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
    }
}