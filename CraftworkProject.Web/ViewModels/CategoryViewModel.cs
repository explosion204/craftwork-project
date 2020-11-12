using System;
using System.Collections.Generic;
using CraftworkProject.Domain.Models;

namespace CraftworkProject.Web.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> AllCategories { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}