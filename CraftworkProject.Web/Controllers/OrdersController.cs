using System;
using System.Linq;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Controllers
{
    // TODO: authorize policy
    public class OrdersController : Controller
    {
        private readonly IDataManager _dataManager;
        private readonly IUserManager _userManager;

        public OrdersController(IDataManager dataManager, IUserManager userManager)
        {
            _dataManager = dataManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> PendingOrders()
        {
            var user = await _userManager.FindUserByName(User.Identity.Name);
            var userOrders = _dataManager.OrderRepository.GetAllEntities()
                .Where(x => x.User.Id == user.Id);
            var pendingUserOrders = userOrders.Where(x => x.Processed && !x.Canceled && !x.Finished).ToList();
            
            ViewData["pendingOrdersCount"] = pendingUserOrders.Count;
            ViewData["canceledOrdersCount"] = userOrders.Count(x => x.Processed && x.Canceled && !x.Finished);
            ViewData["finishedOrdersCount"] = userOrders.Count(x => x.Processed && !x.Canceled && x.Finished);
            ViewData["profileImagePath"] = user.ProfilePicture;
            ViewData["username"] = user.Username;
            
            return View(pendingUserOrders);
        }

        public async Task<IActionResult> CanceledOrders()
        {
            var user = await _userManager.FindUserByName(User.Identity.Name);
            var userOrders = _dataManager.OrderRepository.GetAllEntities()
                .Where(x => x.User.Id == user.Id);
            var canceledOrders = userOrders.Where(x => x.Processed && x.Canceled && !x.Finished).ToList();
            
            ViewData["pendingOrdersCount"] = userOrders.Count(x => x.Processed && !x.Canceled && !x.Finished);
            ViewData["canceledOrdersCount"] = canceledOrders.Count;
            ViewData["finishedOrdersCount"] = userOrders.Count(x => x.Processed && !x.Canceled && x.Finished);
            ViewData["profileImagePath"] = user.ProfilePicture;
            ViewData["username"] = user.Username;
            
            return View(canceledOrders);
        }

        public async Task<IActionResult> FinishedOrders()
        {
            var user = await _userManager.FindUserByName(User.Identity.Name);
            var userOrders = _dataManager.OrderRepository.GetAllEntities()
                .Where(x => x.User.Id == user.Id);
            var finishedOrders = userOrders.Where(x => x.Processed && !x.Canceled && x.Finished).ToList();
            
            ViewData["pendingOrdersCount"] = userOrders.Count(x => x.Processed && !x.Canceled && !x.Finished);
            ViewData["canceledOrdersCount"] = userOrders.Count(x => x.Processed && x.Canceled && !x.Finished);
            ViewData["finishedOrdersCount"] = finishedOrders.Count;
            ViewData["profileImagePath"] = user.ProfilePicture;
            ViewData["username"] = user.Username;
            
            return View(finishedOrders);
        }
        
        public IActionResult GetOrder(string orderId)
        {
            return Json(_dataManager.OrderRepository.GetEntity(Guid.Parse(orderId)));
        }
    }
}