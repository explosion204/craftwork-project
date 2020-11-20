using System;
using System.Linq;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Index(Guid id)
        {
            var product = _dataManager.ProductRepository.GetEntity(id);

            if (product == null)
                return Redirect("/error/404");
            
            var reviews = _dataManager.ReviewRepository.GetAllEntities().Where(x => x.Product.Id == id).ToList();
            var currentUserId = _userManager.GetUserId(User);
            var currentUserReview = reviews.FirstOrDefault(x => x.User.Id == currentUserId);
            var reviewSubmitAllowed = false;
            
            if (User.Identity.IsAuthenticated)
            {
                var userFinishedOrders = _dataManager.OrderRepository.GetAllEntities()
                    .Where(x => x.User.Id == currentUserId && x.Finished).ToList();

                foreach (var order in userFinishedOrders)
                {
                    if (order.PurchaseDetails.FirstOrDefault(x => x.Product.Id == id) != null)
                    {
                        reviewSubmitAllowed = true;
                    }
                }
            }

            var viewModel = new ProductViewModel
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
                ReviewSubmitAllowed = reviewSubmitAllowed,
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
        
        [Authorize]
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
                    PublicationDate = DateTime.Now,
                    User = await _userManager.FindUserById(_userManager.GetUserId(User))
                };
                
                _dataManager.ReviewRepository.SaveEntity(review);
            }

            var errors = ModelState.Values.Select(
                v => v.Errors.Count != 0 ? v.Errors[0].ErrorMessage : string.Empty).ToList();
            TempData["errors"] = errors;

            return Redirect($"/product?id={model.ProductId}");
        }
    }
}