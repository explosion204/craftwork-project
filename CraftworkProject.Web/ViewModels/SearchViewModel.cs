using System.ComponentModel.DataAnnotations;

namespace CraftworkProject.Web.ViewModels
{
    public class SearchViewModel
    {
        [Required]
        public string Query { get; set; }
        public string Filter { get; set; }
    }
}