using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IDataManager _dataManager;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(IDataManager dataManager, IImageService imageService, IWebHostEnvironment environment)
        {
            _dataManager = dataManager;
            _imageService = imageService;
            _environment = environment;
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
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (model.Image != null)
                {
                    fileName = await UploadFile(model.Image);
                } 
                
                var product = new Product
                {
                    Name = model.Name,
                    Category = _dataManager.CategoryRepository.GetEntity(model.CategoryId),
                    ShortDesc = model.ShortDesc,
                    Desc = model.Desc,
                    ImagePath = fileName,
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
        public IActionResult Delete(string id)
        {
            _dataManager.ProductRepository.DeleteEntity(Guid.Parse(id));
            return Json(new {success = true});
        }

        public IActionResult Update(Guid id)
        {
            ViewBag.AllCategories = _dataManager.CategoryRepository.GetAllEntities().ToList();

            var product = _dataManager.ProductRepository.GetEntity(id);
            var viewModel = new ProductViewModel
            {
                Id = id,
                CategoryId = product.Category.Id,
                Name = product.Name,
                ShortDesc = product.ShortDesc,
                Desc = product.Desc,
                Price = product.Price,
                InStock = product.InStock,
                ImagePath = product.ImagePath,
                Rating = product.Rating,
                RatesCount = product.RatesCount
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Id = model.Id,
                    Name = model.Name,
                    Category = _dataManager.CategoryRepository.GetEntity(model.CategoryId),
                    ShortDesc = model.ShortDesc,
                    Desc = model.Desc,
                    InStock = model.InStock,
                    Price = model.Price,
                    ImagePath = model.ImagePath,
                    Rating = model.Rating,
                    RatesCount = model.RatesCount
                };
                
                if (model.Image != null)
                {
                    if (model.ImagePath != null)
                        DeleteFile(model.ImagePath);
                    product.ImagePath = await UploadFile(model.Image);
                }

                _dataManager.ProductRepository.SaveEntity(product);
                return Redirect("/admin/products"); 
            }

            ViewBag.AllCategories = _dataManager.CategoryRepository.GetAllEntities().ToList();
            return View(model);
        }
        
        private async Task<string> UploadFile(IFormFile file)
        {
            string uploadDir = Path.Combine(_environment.WebRootPath, "img/product");

            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            
            string fileName = $"{Guid.NewGuid().ToString()}_{file.FileName}";
            string filePath = Path.Combine(uploadDir, fileName);
            
            await _imageService.SaveImage(file, filePath);
            return fileName;
        }

        private void DeleteFile(string fileName)
        {
            string uploadDir = Path.Combine(_environment.WebRootPath, "img/product");
            string filePath = Path.Combine(uploadDir, fileName);
            
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}