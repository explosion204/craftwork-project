using System;
using System.Collections.Generic;
using System.Linq;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.ViewModels;
using CraftworkProject.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataManager _dataManager;
        public HomeController(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }
        
        public IActionResult Index()
        {
            var allCategories = new List<CategoryViewModel>();
            throw new Exception("Fuck you!");

            foreach (var category in _dataManager.CategoryRepository.GetAllEntities().ToList())
            {
                var count = category.Products.Count >= 5 ? 5 : category.Products.Count;
                allCategories.Add(new CategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name,
                    BestRatedProducts = category.Products
                        .OrderByDescending(x => x.Rating)
                        .Take(count)
                        .ToList()
                });
            }
            
            var viewModel = new HomeViewModel()
            {
                AllCategories = allCategories,
                SearchViewModel = new SearchViewModel()
            };
            
            return View(viewModel);
        }

        public IActionResult Search(HomeViewModel model)
        {
            if (TryValidateModel(model.SearchViewModel))
            {
                return Redirect($"/search?query={model.SearchViewModel.Query}&filter={model.SearchViewModel.Filter}");
            }

            return null;
        }
    }
}