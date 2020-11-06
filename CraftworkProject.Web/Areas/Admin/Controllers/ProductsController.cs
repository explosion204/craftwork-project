using System;
using System.Linq;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IDataManager _dataManager;

        public ProductsController(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public IActionResult Index()
        {
            return View(_dataManager.ProductRepository.GetAllEntities());
        }

        public IActionResult Create()
        {
            ViewBag.AllCategories = _dataManager.CategoryRepository.GetAllEntities().ToList();
            return View(new ProductViewModel());
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Name = model.Name,
                    Category = _dataManager.CategoryRepository.GetEntity(model.CategoryId),
                    Desc = model.Desc,
                    ImagePath = model.ImagePath,
                    InStock = model.InStock,
                    Price = model.Price
                };
                _dataManager.ProductRepository.SaveEntity(product);
                return Redirect("/admin/products");
            }

            ViewBag.AllCategories = _dataManager.CategoryRepository.GetAllEntities().ToList();
            return View(model);
        }
 
        [HttpPost]
        public bool Delete(Guid id)
        {
            try
            {
                _dataManager.ProductRepository.DeleteEntity(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IActionResult Update(Guid id)
        {
            ViewBag.AllCategories = _dataManager.CategoryRepository.GetAllEntities().ToList();

            Product product = _dataManager.ProductRepository.GetEntity(id);
            ProductViewModel viewModel = new ProductViewModel()
            {
                Id = id,
                CategoryId = product.Category.Id,
                Name = product.Name,
                Desc = product.Desc,
                Price = product.Price,
                InStock = product.InStock,
                ImagePath = product.ImagePath
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Update(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Category = _dataManager.CategoryRepository.GetEntity(model.CategoryId),
                    Desc = model.Desc,
                    ImagePath = model.ImagePath,
                    InStock = model.InStock,
                    Price = model.Price
                };
                _dataManager.ProductRepository.SaveEntity(product);
                return Redirect("/admin/products"); 
            }

            ViewBag.AllCategories = _dataManager.CategoryRepository.GetAllEntities().ToList();
            return View(model);
        }
    }
}