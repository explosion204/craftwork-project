using System;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IDataManager _dataManager;

        public CategoriesController(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }
        
        public IActionResult Index()
        {
            return View(_dataManager.CategoryRepository.GetAllEntities());
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
                _dataManager.CategoryRepository.SaveEntity(category);
                return Redirect("/admin/categories");
            }

            return View(category);
        }

        [HttpPost]
        public bool Delete(Guid id)
        {
            try
            {
                _dataManager.CategoryRepository.DeleteEntity(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public IActionResult Update(Guid id)
        {
            return View(_dataManager.CategoryRepository.GetEntity(id));
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                _dataManager.CategoryRepository.SaveEntity(category);
                return Redirect("/admin/categories");
            }

            return View(category);
        }
    }
}