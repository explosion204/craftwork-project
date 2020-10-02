using System;
using System.Collections;
using System.Collections.Generic;
using CraftworkProject.Domain;
using Microsoft.AspNetCore.Identity;

namespace CraftworkProject.Domain.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}