using AetheriaWebService.DataAccess;
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
        private readonly IAetheriaHelper _aetheriaHelper;
        public AetheriaHub(IAetheriaHelper aetheriaHelper)
        {
            _aetheriaHelper = aetheriaHelper;
        }
        public Task Send(string message)
        {
            var clientMessage = JsonConvert.DeserializeObject<AetheriaClientMessage>(message);

            var response = _aetheriaHelper.ProcessPlayerInput(clientMessage.Message, clientMessage.ChatUserId, clientMessage.ChatUsername, clientMessage.Platform);
            if (response != "")
            {
                var relevantChatUsers = new List<ChatUserDTO>();
                relevantChatUsers.Add(new ChatUserDTO
                {
                    ChatUserId = clientMessage.ChatUserId,
                    Platform = clientMessage.Platform
                });
                var serverResponse = new AetheriaServerResponse
                {
                    ServerAuthToken = "TestToken",
                    RelevantChatUsers = relevantChatUsers,
                    Response = response
                };
                var responseMessage = JsonConvert.SerializeObject(serverResponse);
                return Clients.All.InvokeAsync("Send", responseMessage);
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        public Task ServerSend(string message)
        {
            return Clients.All.InvokeAsync("ServerSend", message);
        }
    }
}
