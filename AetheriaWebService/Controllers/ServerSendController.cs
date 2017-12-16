using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MUDService.Hubs;
using Microsoft.AspNetCore.SignalR;
using MUDService.Models;
using MUDService.Helpers;

namespace MUDService.Controllers
{
    [Produces("application/json")]
    [Route("api/ServerSend")]
    public class ServerSendController : Controller
    {
        private IHubContext<MUDHub> _messageHubContext;

        public ServerSendController(IHubContext<MUDHub> messageHubContext)
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