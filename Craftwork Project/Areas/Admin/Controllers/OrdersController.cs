using System;
using System.Linq;
using Craftwork_Project.Domain;
using Craftwork_Project.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Craftwork_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private DataManager dataManager;
        private UserManager<ApplicationUser> userManager;

        public OrdersController(DataManager dataManager, UserManager<ApplicationUser> userManager)
        {
            this.dataManager = dataManager;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(dataManager.Orders.GetAllOrders());
        }

        public IActionResult Create()
        {
            ViewBag.AllUsers = userManager.Users.ToList();
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                dataManager.Orders.SaveOrder(order);
                return Redirect("/admin/orders");
            }
            
            ViewBag.AllUsers = userManager.Users.ToList();
            return View(order);
        }
        
        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                dataManager.Orders.DeleteOrder(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public IActionResult Update(int id)
        {
            ViewBag.AllUsers = userManager.Users.ToList();
            return View(dataManager.Orders.GetOrder(id));
        }
        
        [HttpPost]
        public IActionResult Update(Order order)
        {
            if (ModelState.IsValid)
            {
                dataManager.Orders.SaveOrder(order);
                return Redirect("/admin/orders");
            }
            
            ViewBag.AllUsers = userManager.Users.ToList();
            return View(order);
        }
    }
}