using System;
using System.Linq;
using CraftworkProject.Domain;
using CraftworkProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
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
            return View(dataManager.PurchaseDetails.GetAllEntities());
        }

        public IActionResult Create()
        {
            ViewBag.AllOrders = dataManager.Orders.GetAllEntities().ToList();
            ViewBag.AllProducts = dataManager.Products.GetAllEntities().ToList();
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(PurchaseDetail detail)
        {
            if (ModelState.IsValid)
            {
                dataManager.PurchaseDetails.SaveEntity(detail);
                return Redirect("/admin/purchasedetails");
            }
            
            ViewBag.AllOrders = dataManager.Orders.GetAllEntities().ToList();
            ViewBag.AllProducts = dataManager.Products.GetAllEntities().ToList();
            return View(detail);
        }
        
        [HttpPost]
        public bool Delete(Guid id)
        {
            try
            {
                dataManager.PurchaseDetails.DeleteEntity(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public IActionResult Update(Guid id)
        {
            ViewBag.AllOrders = dataManager.Orders.GetAllEntities().ToList();
            ViewBag.AllProducts = dataManager.Products.GetAllEntities().ToList();
            return View(dataManager.PurchaseDetails.GetEntity(id));
        }
        
        [HttpPost]
        public IActionResult Update(PurchaseDetail detail)
        {
            if (ModelState.IsValid)
            {
                dataManager.PurchaseDetails.SaveEntity(detail);
                return Redirect("/admin/purchasedetails");
            }
            
            ViewBag.AllOrders = dataManager.Orders.GetAllEntities().ToList();
            ViewBag.AllProducts = dataManager.Products.GetAllEntities().ToList();
            return View(detail);
        }
    }
}