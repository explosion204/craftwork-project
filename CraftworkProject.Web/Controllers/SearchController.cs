using System;
using System.Linq;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.ViewModels;
using CraftworkProject.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    public class SearchController : Controller
    {
        private const int PageSize = 9;

        private readonly IDataManager _dataManager;
        
        public SearchController(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }
        
        public IActionResult Index(string query, string filter, string order = "highestRating", int page = 1)
        {
            var products = order switch
            {
                "highestRating" => _dataManager.ProductRepository.GetAllEntities()
                    .Where(x => x.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(x => x.Rating)
                    .ToList(),
                
                "lowestRating" => _dataManager.ProductRepository.GetAllEntities()
                    .Where(x => x.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase))
                    .OrderBy(x => x.Rating)
                    .ToList(),
                
                "highestPrice" => _dataManager.ProductRepository.GetAllEntities()
                    .Where(x => x.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(x => x.Price)
                    .ToList(),
                
                _ => _dataManager.ProductRepository.GetAllEntities()
                    .Where(x => x.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase))
                    .OrderBy(x => x.Price)
                    .ToList()
            };

            if (filter != null)
            {
                products = products.Where(x => x.Category.Id == Guid.Parse(filter)).ToList();
            }

            var pageViewModel = new PageViewModel(products.Count, page, PageSize);
            var viewModel = new ListViewModel
            {
                CategoryId = default,
                ItemOrdering = order,
                Products = products.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                AllCategories = _dataManager.CategoryRepository.GetAllEntities(),
                PageViewModel = pageViewModel,
                SearchViewModel = new SearchViewModel
                {
                    Query = query,
                    Filter = filter
                }
            };

            return View("~/Views/ListView.cshtml", viewModel);
        }
    }
}