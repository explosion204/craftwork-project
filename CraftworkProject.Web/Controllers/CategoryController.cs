using System;
using System.Linq;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    public class CategoryController : Controller
    {
        private const int PageSize = 2;

        private readonly IDataManager _dataManager;
        
        public CategoryController(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }
        
        public IActionResult Index(Guid id, string order = "highestRating", int page = 1)
        {
            var category = _dataManager.CategoryRepository.GetEntity(id);

            var products = order switch
            {
                "highestRating" => category.Products
                    .Where(x => x.InStock)
                    .OrderByDescending(x => x.Rating)
                    .ToList(),
                
                "lowestRating" => category.Products
                    .Where(x => x.InStock)
                    .OrderBy(x => x.Rating)
                    .ToList(),
                
                "highestPrice" => category.Products
                    .Where(x => x.InStock)
                    .OrderByDescending(x => x.Price)
                    .ToList(),
                
                _ => category.Products
                    .Where(x => x.InStock)
                    .OrderBy(x => x.Price)
                    .ToList()
            };

            var pageViewModel = new PageViewModel(products.Count, page, PageSize);
            var viewModel = new ListViewModel()
            {
                CategoryId = id,
                Name = category.Name,
                Desc = category.Desc,
                ItemOrdering = order,
                Products = products.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                AllCategories = _dataManager.CategoryRepository.GetAllEntities(),
                PageViewModel = pageViewModel
            };
            
            return View("~/Views/ListView.cshtml", viewModel);
        }
    }
}