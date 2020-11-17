using System.Collections.Generic;
using CraftworkProject.Web.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.ViewModels.Cart
{
    public class CartViewModel
    {
        public List<Domain.Models.Product> Products { get; set; }
        public List<int> Quantities { get; set; }
        public bool MakeOrderAllowed { get; set; }
    }
}