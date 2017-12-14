using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AetheriaWebService.Hubs;
using Microsoft.AspNetCore.SignalR;
using AetheriaWebService.Models;
using AetheriaWebService.Helpers;

namespace AetheriaWebService.Controllers
{
    [Produces("application/json")]
    [Route("api/ServerSend")]
    public class ServerSendController : Controller
    {
        private IHubContext<AetheriaHub> _messageHubContext;

        public ServerSendController(IHubContext<AetheriaHub> messageHubContext)
        {
            _messageHubContext = messageHubContext;
        }

        [HttpPost]
        public void Send(string value, List<ChatUser> chatUsers)
        {
            var replicationHelper = new ReplicationHelper(_messageHubContext);
            replicationHelper.ReplicateToClients(value, chatUsers);
        }
    }
}