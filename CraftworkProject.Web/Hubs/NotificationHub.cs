using System;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CraftworkProject.Web.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly IUserManagerHelper _helper;

        public NotificationHub(IUserConnectionManager userConnectionManager, IUserManagerHelper helper)
        {
            _userConnectionManager = userConnectionManager;
            _helper = helper;
        }

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var userId = _helper.GetUserId(Context.User);
            _userConnectionManager.KeepUserConnection(userId, connectionId);

            return Task.FromResult(0);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveUserConnection(connectionId);

            return Task.FromResult(0);
        }
    }
}