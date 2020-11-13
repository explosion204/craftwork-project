using System;
using System.Collections.Generic;

namespace CraftworkProject.Web.ViewModels.Category
{
    public class ListViewModel
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ItemOrdering { get; set; }
        public List<Domain.Models.Product> Products { get; set; }
        public List<Domain.Models.Category> AllCategories { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SearchViewModel SearchViewModel { get; set; }
    }
}