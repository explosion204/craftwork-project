using System;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IDataManager _dataManager;
        private readonly IUserManager _userManager;
        
        public OrdersController(IDataManager dataManager, IUserManager userManager)
        {
            _dataManager = dataManager;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            return View(_dataManager.OrderRepository.GetAllEntities());
        }

        public IActionResult Create()
        {
            ViewBag.AllUsers = _userManager.GetAllUsers();
            return View(new OrderViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindUserById(model.UserId);
                Order order = new Order()
                {
                    Created = DateTime.UtcNow,
                    Processed = model.Processed,
                    Canceled = model.Canceled,
                    Finished = model.Finished,
                    User = user
                };
                _dataManager.OrderRepository.SaveEntity(order);
                return Redirect("/admin/orders");
            }

            ViewBag.AllUsers = _userManager.GetAllUsers();
            return View(model);
        }
        
        [HttpPost]
        public IActionResult Delete(string id)
        {
            _dataManager.OrderRepository.DeleteEntity(Guid.Parse(id));
            return Json(new {success = true});
        }
        
        public IActionResult Update(Guid id)
        {
            ViewBag.AllUsers = _userManager.GetAllUsers();

            Order order = _dataManager.OrderRepository.GetEntity(id);
            OrderViewModel viewModel = new OrderViewModel()
            {
                Id = order.Id,
                Created = order.Created,
                UserId = order.User.Id,
                Finished = order.Finished,
                Canceled = order.Canceled,
                Processed = order.Processed
            };
            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindUserById(model.UserId);
                Order order = new Order()
                {
                    Id = model.Id,
                    Created = model.Created,
                    Processed = model.Processed,
                    Canceled = model.Canceled,
                    Finished = model.Finished,
                    User = user
                };
                _dataManager.OrderRepository.SaveEntity(order);
                return Redirect("/admin/orders");
            }

            ViewBag.AllUsers = _userManager.GetAllUsers();
            return View(model);
        }
    }
}