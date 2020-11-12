using System;
using System.Linq;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IDataManager _dataManager;
        
        public CategoryController(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }
        
        public IActionResult Index(Guid id, int page = 1)
        {
            const int pageSize = 12;
            
            var category = _dataManager.CategoryRepository.GetEntity(id);
            var products = category.Products.Where(x => x.InStock).ToList();
            var pageViewModel = new PageViewModel(products.Count, page, pageSize);
            var viewModel = new CategoryViewModel()
            {
                Id = id,
                Name = category.Name,
                Desc = category.Desc,
                Products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                AllCategories = _dataManager.CategoryRepository.GetAllEntities(),
                PageViewModel = pageViewModel
            };
            
            return View(viewModel);
        }
    }
}