using System;
using System.Threading.Tasks;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CraftworkProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagingController : Controller
    {
        private readonly IDataManager _dataManager;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IUserConnectionManager _userConnectionManager;
        
        public ManagingController(
            IDataManager dataManager, 
            IHubContext<NotificationHub> notificationHubContext,
            IUserConnectionManager userConnectionManager)
        {
            _dataManager = dataManager;
            _notificationHubContext = notificationHubContext;
            _userConnectionManager = userConnectionManager;
        }
        
        public IActionResult Index()
        {
            return View(_dataManager.OrderRepository.GetAllEntities());
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(string id)
        {
            var order = _dataManager.OrderRepository.GetEntity(Guid.Parse(id));
            order.Processed = true;
            _dataManager.OrderRepository.SaveEntity(order);

            await SendNotification(order.User.Id, "notifyOrderConfirmed");

            return Json(new {success = true});
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(string id)
        {
            var order = _dataManager.OrderRepository.GetEntity(Guid.Parse(id));
            order.Canceled = true;
            _dataManager.OrderRepository.SaveEntity(order);
            
            await SendNotification(order.User.Id, "notifyOrderCanceled");

            return Json(new {success = true});
        }

        [HttpPost]
        public async Task<IActionResult> RestoreOrder(string id)
        {
            var order = _dataManager.OrderRepository.GetEntity(Guid.Parse(id));
            order.Canceled = false;
            _dataManager.OrderRepository.SaveEntity(order);
            
            await SendNotification(order.User.Id, "notifyOrderRestored");

            return Json(new {success = true});
        }

        [HttpPost]
        public async Task<IActionResult> FinishOrder(string id)
        {
            var order = _dataManager.OrderRepository.GetEntity(Guid.Parse(id));
            order.Finished = true;
            _dataManager.OrderRepository.SaveEntity(order);

            await SendNotification(order.User.Id, "notifyOrderFinished");

            return Json(new {success = true});
        }

        private async Task SendNotification(Guid userId, string methodName)
        {
            var connections = _userConnectionManager.GetUserConnections(userId);
            
            if (connections != null && connections.Count > 0)
            {
                foreach (var connectionId in connections)
                {
                    await _notificationHubContext.Clients.Client(connectionId).SendAsync(methodName);
                }
            }
        }
    }
}