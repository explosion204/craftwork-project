using System;
using System.Linq;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Identity;
using CraftworkProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
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
            return View(dataManager.Orders.GetAllEntities());
        }

        public IActionResult Create()
        {
            ViewBag.AllUsers = userManager.Users.ToList();
            ViewBag.UserManager = userManager;    
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                dataManager.Orders.SaveEntity(order);
                return Redirect("/admin/orders");
            }
            
            ViewBag.AllUsers = userManager.Users.ToList();
            return View(order);
        }
        
        [HttpPost]
        public bool Delete(Guid id)
        {
            try
            {
                dataManager.Orders.DeleteEntity(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public IActionResult Update(Guid id)
        {
            ViewBag.AllUsers = userManager.Users.ToList();
            return View(dataManager.Orders.GetEntity(id));
        }
        
        [HttpPost]
        public IActionResult Update(Order order)
        {
            if (ModelState.IsValid)
            {
                dataManager.Orders.SaveEntity(order);
                return Redirect("/admin/orders");
            }
            
            ViewBag.AllUsers = userManager.Users.ToList();
            return View(order);
        }
    }
}