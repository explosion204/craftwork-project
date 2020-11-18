using System;
using CraftworkProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagingController : Controller
    {
        private readonly IDataManager _dataManager;
        
        public ManagingController(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }
        
        public IActionResult Index()
        {
            return View(_dataManager.OrderRepository.GetAllEntities());
        }

        [HttpPost]
        public IActionResult ConfirmOrder(string id)
        {
            var order = _dataManager.OrderRepository.GetEntity(Guid.Parse(id));
            order.Processed = true;
            _dataManager.OrderRepository.SaveEntity(order);

            return Json(new {success = true});
        }

        [HttpPost]
        public IActionResult CancelOrder(string id)
        {
            var order = _dataManager.OrderRepository.GetEntity(Guid.Parse(id));
            order.Canceled = true;
            _dataManager.OrderRepository.SaveEntity(order);

            return Json(new {success = true});
        }

        [HttpPost]
        public IActionResult RestoreOrder(string id)
        {
            var order = _dataManager.OrderRepository.GetEntity(Guid.Parse(id));
            order.Canceled = false;
            _dataManager.OrderRepository.SaveEntity(order);

            return Json(new {success = true});
        }

        [HttpPost]
        public IActionResult FinishOrder(string id)
        {
            var order = _dataManager.OrderRepository.GetEntity(Guid.Parse(id));
            order.Finished = true;
            _dataManager.OrderRepository.SaveEntity(order);

            return Json(new {success = true});
        }
    }
}