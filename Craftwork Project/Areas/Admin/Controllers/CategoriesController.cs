using System;
using System.Linq;
using Craftwork_Project.Domain;
using Craftwork_Project.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Craftwork_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly DataManager dataManager;

        public CategoriesController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        
        public IActionResult Index()
        {
            return View(dataManager.Categories.GetAllCategories());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                dataManager.Categories.SaveCategory(category);
                return Redirect("/admin/categories");
            }

            return View(category);
        }

        [HttpPost]
        public bool Delete(Guid id)
        {
            try
            {
                dataManager.Categories.DeleteCategory(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public IActionResult Update(Guid id)
        {
            return View(dataManager.Categories.GetCategory(id));
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                dataManager.Categories.SaveCategory(category);
                return RedirectToAction("Index", "Categories");
            }

            return View(category);
        }
    }
}