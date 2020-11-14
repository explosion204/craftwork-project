using System;
using System.Linq;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PurchaseDetailsController : Controller
    {
        private IDataManager _dataManager;

        public PurchaseDetailsController(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public IActionResult Index()
        {
            return View(_dataManager.PurchaseDetailRepository.GetAllEntities());
        }

        public IActionResult Create()
        {
            ViewBag.AllOrders = _dataManager.OrderRepository.GetAllEntities().ToList();
            ViewBag.AllProducts = _dataManager.ProductRepository.GetAllEntities().ToList();

            return View(new PurchaseDetailViewModel());
        }
        
        [HttpPost]
        public IActionResult Create(PurchaseDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                PurchaseDetail detail = new PurchaseDetail()
                {
                    OrderId = model.Id,
                    Product = _dataManager.ProductRepository.GetEntity(model.ProductId),
                    Amount = model.Amount
                };
                _dataManager.PurchaseDetailRepository.SaveEntity(detail);
                return Redirect("/admin/purchasedetails");
            }
            
            ViewBag.AllOrders = _dataManager.OrderRepository.GetAllEntities().ToList();
            ViewBag.AllProducts = _dataManager.ProductRepository.GetAllEntities().ToList();
            return View(model);
        }
        
        [HttpPost]
        public bool Delete(Guid id)
        {
            try
            {
                _dataManager.PurchaseDetailRepository.DeleteEntity(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public IActionResult Update(Guid id)
        {
            ViewBag.AllOrders = _dataManager.OrderRepository.GetAllEntities().ToList();
            ViewBag.AllProducts = _dataManager.ProductRepository.GetAllEntities().ToList();

            PurchaseDetail detail = _dataManager.PurchaseDetailRepository.GetEntity(id);
            PurchaseDetailViewModel viewModel = new PurchaseDetailViewModel()
            {
                Id = id,
                OrderId = detail.OrderId,
                ProductId = detail.Product.Id,
                Amount = detail.Amount
            };
            
            return View(viewModel);
        }
        
        [HttpPost]
        public IActionResult Update(PurchaseDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                PurchaseDetail detail = new PurchaseDetail()
                {
                    Id = model.Id,
                    OrderId = model.OrderId,
                    Product = _dataManager.ProductRepository.GetEntity(model.ProductId),
                    Amount = model.Amount
                };
                _dataManager.PurchaseDetailRepository.SaveEntity(detail);
                return Redirect("/admin/purchasedetails");
            }
            
            ViewBag.AllOrders = _dataManager.OrderRepository.GetAllEntities().ToList();
            ViewBag.AllProducts = _dataManager.ProductRepository.GetAllEntities().ToList();
            return View(model);
        }
    }
}