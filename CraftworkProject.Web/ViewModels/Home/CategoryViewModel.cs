using System;
using System.Collections.Generic;

namespace CraftworkProject.Web.ViewModels.Home
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Domain.Models.Product> BestRatedProducts { get; set; }
    }
}