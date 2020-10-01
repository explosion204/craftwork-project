using System;
using System.Linq;
using Craftwork_Project.Domain;
using Craftwork_Project.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Craftwork_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PurchaseDetailsController : Controller
    {
        private DataManager dataManager;

        public PurchaseDetailsController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public IActionResult Index()
        {
            return View(dataManager.PurchaseDetails.GetAllPurchaseDetails());
        }

        public IActionResult Create()
        {
            ViewBag.AllOrders = dataManager.Orders.GetAllOrders().ToList();
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
            
            ViewBag.AllOrders = dataManager.Orders.GetAllOrders().ToList();
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
            ViewBag.AllOrders = dataManager.Orders.GetAllOrders().ToList();
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
            
            ViewBag.AllOrders = dataManager.Orders.GetAllOrders().ToList();
            ViewBag.AllProducts = dataManager.Products.GetAllProducts().ToList();
            return View(detail);
        }
    }
}