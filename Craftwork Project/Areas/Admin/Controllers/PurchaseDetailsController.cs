using System;
using System.Linq;
using Craftwork_Project.Domain;
using Craftwork_Project.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Craftwork_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PurchaseDetailsController : Controller
    {
        private DataManager dataManager;
        private UserManager<ApplicationUser> userManager;

        public PurchaseDetailsController(DataManager dataManager, UserManager<ApplicationUser> userManager)
        {
            this.dataManager = dataManager;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(dataManager.PurchaseDetails.GetAllPurchaseDetails());
        }

        public IActionResult Create()
        {
            ViewBag.AllUsers = userManager.Users.ToList();
            ViewBag.AllProducts = dataManager.Products.GetAllProducts().ToList();
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(PurchaseDetail detail)
        {
            if (ModelState.IsValid)
            {
                dataManager.PurchaseDetails.SavePurchaseDetail(detail);
                return Redirect("/admin/purchasedetails");
            }
            
            ViewBag.AllUsers = userManager.Users.ToList();
            ViewBag.AllProducts = dataManager.Products.GetAllProducts().ToList();
            return View(detail);
        }
        
        [HttpPost]
        public bool Delete(Guid id)
        {
            try
            {
                dataManager.PurchaseDetails.DeletePurchaseDetail(id);
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
            ViewBag.AllProducts = dataManager.Products.GetAllProducts().ToList();
            return View(dataManager.PurchaseDetails.GetPurchaseDetail(id));
        }
        
        [HttpPost]
        public IActionResult Update(PurchaseDetail detail)
        {
            if (ModelState.IsValid)
            {
                dataManager.PurchaseDetails.SavePurchaseDetail(detail);
                return Redirect("/admin/purchasedetails");
            }
            
            ViewBag.AllUsers = userManager.Users.ToList();
            ViewBag.AllProducts = dataManager.Products.GetAllProducts().ToList();
            return View(detail);
        }
    }
}