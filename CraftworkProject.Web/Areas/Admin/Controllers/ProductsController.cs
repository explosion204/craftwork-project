using System;
using System.Linq;
using CraftworkProject.Domain;
using CraftworkProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
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
            return View(dataManager.Products.GetAllEntities());
        }

        public IActionResult Create()
        {
            ViewBag.AllCategories = dataManager.Categories.GetAllEntities().ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                dataManager.Products.SaveEntity(product);
                return Redirect("/admin/products");
            }

            ViewBag.AllCategories = dataManager.Categories.GetAllEntities().ToList();
            return View(product);
        }
 
        [HttpPost]
        public bool Delete(Guid id)
        {
            try
            {
                dataManager.Products.DeleteEntity(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IActionResult Update(Guid id)
        {
            ViewBag.AllCategories = dataManager.Categories.GetAllEntities().ToList();
            return View(dataManager.Products.GetEntity(id));
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                dataManager.Products.SaveEntity(product);
                return Redirect("/admin/products"); 
            }

            ViewBag.AllCategories = dataManager.Categories.GetAllEntities().ToList();
            return View(product);
        }
    }
}