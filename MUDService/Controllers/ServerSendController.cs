using Microsoft.AspNetCore.Mvc;
using MUDService.Logic;
using MUDService.Models;
using System.Collections.Generic;

namespace MUDService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ServerSendController : Controller
    {
        private readonly IReplicationLogic _replicationLogic;

        public ServerSendController(IReplicationLogic replicationLogic)
        {
            _replicationLogic = replicationLogic;
        }

        [HttpPost]
        public void Send(string value, List<ChatUser> chatUsers)
        {
            _replicationLogic.ReplicateToClients(value, chatUsers);
        }
    }
}