using System.Collections.Generic;

namespace CraftworkProject.Web.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<CategoryViewModel> AllCategories { get; set; }
        public SearchViewModel SearchViewModel { get; set; }
    }
}