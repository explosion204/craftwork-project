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
                User user = await _userManager.FindUser(model.UserId);
                Order order = new Order()
                {
                    Processed = model.Processed,
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
        public bool Delete(Guid id)
        {
            try
            {
                _dataManager.OrderRepository.DeleteEntity(id);
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

            Order order = _dataManager.OrderRepository.GetEntity(id);
            OrderViewModel viewModel = new OrderViewModel()
            {
                Id = order.Id,
                UserId = order.User.Id,
                Finished = order.Finished,
                Processed = order.Processed
            };
            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Update(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindUser(model.UserId);
                Order order = new Order()
                {
                    Id = model.Id,
                    Processed = model.Processed,
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