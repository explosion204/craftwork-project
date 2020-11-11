using System;
using System.Linq;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReviewsController : Controller
    {
       private readonly IDataManager _dataManager;
        private readonly IUserManager _userManager;
        
        public ReviewsController(IDataManager dataManager, IUserManager userManager)
        {
            _dataManager = dataManager;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            return View(_dataManager.ReviewRepository.GetAllEntities());
        }

        public IActionResult Create()
        {
            ViewBag.AllUsers = _userManager.GetAllUsers();
            ViewBag.AllProducts = _dataManager.ProductRepository.GetAllEntities().ToList();
            return View(new ReviewViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(ReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Rating < 1 || model.Rating > 5)
                {
                    ModelState.AddModelError(nameof(ReviewViewModel.Rating), "Rating value must be 1, 2, 3, 4 or 5");
                }
                
                var existingReview = _dataManager.ReviewRepository.GetAllEntities()
                    .FirstOrDefault(x => x.Product.Id == model.ProductId && x.User.Id == model.UserId);

                if (existingReview != null)
                {
                    ModelState.AddModelError(nameof(ReviewViewModel.UserId), "User can have only one review to certain product");
                }

                if (ModelState.ErrorCount == 0)
                {
                    User user = await _userManager.FindUserById(model.UserId);
                    Product product = _dataManager.ProductRepository.GetEntity(model.ProductId);
                    Review review = new Review()
                    {
                        User = user,
                        Product = product,
                        Rating = model.Rating,
                        Title = model.Title,
                        Text = model.Text,
                        PublicationDate = DateTime.Now
                    };
                
                    _dataManager.ReviewRepository.SaveEntity(review);
                    return Redirect("/admin/reviews");
                }
            }

            ViewBag.AllUsers = _userManager.GetAllUsers();
            ViewBag.AllProducts = _dataManager.ProductRepository.GetAllEntities().ToList();
            return View(model);
        }
        
        [HttpPost]
        public bool Delete(Guid id)
        {
            try
            {
                _dataManager.ReviewRepository.DeleteEntity(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public IActionResult Update(Guid id)
        {
            ViewBag.AllUsers = _userManager.GetAllUsers();
            ViewBag.AllProducts = _dataManager.ProductRepository.GetAllEntities().ToList();

            var review = _dataManager.ReviewRepository.GetEntity(id);
            ReviewViewModel viewModel = new ReviewViewModel()
            {
                Id = id,
                Title = review.Title,
                Text = review.Text,
                Rating = review.Rating,
                ProductId = review.Product.Id,
                UserId = review.User.Id
            };
            
            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(ReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Rating < 1 || model.Rating > 5)
                {
                    ModelState.AddModelError(nameof(ReviewViewModel.Rating), "Rating value must be 1, 2, 3, 4 or 5");
                }

                if (ModelState.ErrorCount == 0)
                {
                    User user = await _userManager.FindUserById(model.UserId);
                    Product product = _dataManager.ProductRepository.GetEntity(model.ProductId);
                    Review review = new Review()
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Text = model.Text,
                        Rating = model.Rating,
                        Product = product,
                        User = user
                    };
                
                    _dataManager.ReviewRepository.SaveEntity(review);
                    return Redirect("/admin/reviews");
                }
            }

            ViewBag.AllUsers = _userManager.GetAllUsers();
            ViewBag.AllProducts = _dataManager.ProductRepository.GetAllEntities().ToList();
            return View(model);
        }
    }
}