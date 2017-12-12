using AetheriaWebService.Helpers;
using AetheriaWebService.Models;
using AetheriaWebService.ServiceModels;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AetheriaWebService.Hubs
{
    public class AetheriaHub : Hub
    {
        private AetheriaContext db;
        public AetheriaHub(AetheriaContext context)
        {
            db = context;
        }
        public Task Send(string message)
        {
            var clientMessage = JsonConvert.DeserializeObject<AetheriaClientMessage>(message);

            var aetheriaDataAccess = new DataAccess.AetheriaDataAccess(db);
            var aetheriaHelper = new AetheriaHelper(aetheriaDataAccess);
            var player = aetheriaDataAccess.GetPlayer(clientMessage.ChatUserId, clientMessage.Platform);
            var response = aetheriaHelper.ProcessPlayerInput(clientMessage.Message, player);
            var serverResponse = new AetheriaServerResponse
            {
                ServerAuthToken = "TestToken",
                Platform = clientMessage.Platform,
                ChatUserId = clientMessage.ChatUserId,
                Response = response
            };
            var responseMessage = JsonConvert.SerializeObject(serverResponse);
            return Clients.All.InvokeAsync("Send", responseMessage);
        }

        //public Task SendToClient(string message, string connectionId)
        //{
        //    return Clients.Client(connectionId).InvokeAsync("Send", message); 
        //}
    }
}
