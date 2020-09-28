using System;
using System.Linq;
using Craftwork_Project.Domain;
using Craftwork_Project.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Craftwork_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private DataManager dataManager;

        public ProductsController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public IActionResult Index()
        {
            return View(dataManager.Products.GetAllProducts());
        }

        public IActionResult Create()
        {
            ViewBag.AllCategories = dataManager.Categories.GetAllCategories().ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                dataManager.Products.SaveProduct(product);
                return Redirect("/admin/products");
            }

            ViewBag.AllCategories = dataManager.Categories.GetAllCategories().ToList();
            return View(product);
        }
 
        [HttpPost]
        public bool Delete(Guid id)
        {
            try
            {
                dataManager.Products.DeleteProduct(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IActionResult Update(Guid id)
        {
            ViewBag.AllCategories = dataManager.Categories.GetAllCategories().ToList();
            return View(dataManager.Products.GetProduct(id));
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                dataManager.Products.SaveProduct(product);
                return Redirect("/admin/products"); 
            }

            ViewBag.AllCategories = dataManager.Categories.GetAllCategories().ToList();
            return View(product);
        }
    }
}