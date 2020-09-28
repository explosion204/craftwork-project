using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Craftwork_Project.Domain.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}