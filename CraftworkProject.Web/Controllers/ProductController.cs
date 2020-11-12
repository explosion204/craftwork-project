using System;
using System.Linq;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDataManager _dataManager;
        private readonly IUserManager _userManager;
        
        public ProductController(IDataManager dataManager, IUserManager userManager)
        {
            _dataManager = dataManager;
            _userManager = userManager;
        }
        public IActionResult Detail(Guid id)
        {
            Product product = _dataManager.ProductRepository.GetEntity(id);
            var reviews = _dataManager.ReviewRepository.GetAllEntities().Where(x => x.Product.Id == id).ToList();
            var currentUserId = _userManager.GetUserId(User);
            var currentUserReview = reviews.FirstOrDefault(x => x.User.Id == currentUserId);

            var viewModel = new ProductViewModel()
            {
                ProductId = id,
                Name = product.Name,
                Desc = product.Desc,
                Price = product.Price,
                InStock = product.InStock,
                ImagePath = product.ImagePath,
                Category = product.Category,
                CurrentUserReviewExists = currentUserReview != null,
                Rating = product.Rating,
                RatesCount = product.RatesCount,
                Reviews = reviews,
                ReviewId = currentUserReview?.Id ?? default,
                ReviewTitle = currentUserReview?.Title ?? "",
                ReviewText = currentUserReview?.Text ?? "",
                ReviewRating = currentUserReview?.Rating ?? 5
            };
            
            if (TempData["errors"] != null)
            {
                var errors = (string[]) TempData["errors"];
                foreach (var error in errors)
                {
                    if (error.Contains("title", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModelState.AddModelError(nameof(viewModel.ReviewTitle), error);
                    }
                    
                    if (error.Contains("text", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ModelState.AddModelError(nameof(viewModel.ReviewText), error);
                    }
                }
            }
            
            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> SubmitReview(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var review = new Review()
                {
                    Id = model.ReviewId,
                    Title = model.ReviewTitle,
                    Text = model.ReviewText,
                    Rating = int.Parse(Request.Cookies["userRating"]),
                    Product = _dataManager.ProductRepository.GetEntity(model.ProductId),
                    User = await _userManager.FindUserById(_userManager.GetUserId(User))
                };
                
                _dataManager.ReviewRepository.SaveEntity(review);
            }

            var errors = ModelState.Values.Select(
                v => v.Errors.Count != 0 ? v.Errors[0].ErrorMessage : string.Empty).ToList();
            TempData["errors"] = errors;

            return Redirect($"/product/detail?id={model.ProductId}");
        }
    }
}