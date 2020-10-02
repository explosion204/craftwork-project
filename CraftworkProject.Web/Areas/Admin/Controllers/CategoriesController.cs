using System;
using CraftworkProject.Domain;
using CraftworkProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
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
            return View(dataManager.Categories.GetAllEntities());
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
                dataManager.Categories.SaveEntity(category);
                return Redirect("/admin/categories");
            }

            return View(category);
        }

        [HttpPost]
        public bool Delete(Guid id)
        {
            try
            {
                dataManager.Categories.DeleteEntity(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public IActionResult Update(Guid id)
        {
            return View(dataManager.Categories.GetEntity(id));
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                dataManager.Categories.SaveEntity(category);
                return Redirect("/admin/categories");
            }

            return View(category);
        }
    }
}