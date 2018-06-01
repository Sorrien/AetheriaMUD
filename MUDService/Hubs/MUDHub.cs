using MUDService.DataAccess;
using MUDService.Helpers;
using MUDService.Models;
using MUDService.ServiceModels;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDService.Hubs
{
    public class MUDHub : Hub
    {
        private readonly IMUDHelper _mudHelper;
        public MUDHub(IMUDHelper mudHelper)
        {
            _mudHelper = mudHelper;
        }
        public Task Send(string message)
        {
            var clientMessage = JsonConvert.DeserializeObject<MUDClientMessage>(message);

            var response = _mudHelper.ProcessPlayerInput(clientMessage.Message, clientMessage.ChatUserId, clientMessage.ChatUsername, clientMessage.Platform);
            if (response != "")
            {
                var relevantChatUsers = new List<ChatUserDTO>();
                relevantChatUsers.Add(new ChatUserDTO
                {
                    ChatUserId = clientMessage.ChatUserId,
                    Platform = clientMessage.Platform
                });
                var serverResponse = new MUDServerResponse
                {
                    ServerAuthToken = "TestToken",
                    RelevantChatUsers = relevantChatUsers,
                    Response = response
                };
                var responseMessage = JsonConvert.SerializeObject(serverResponse);
                return Clients.All.SendAsync("Send", responseMessage);
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        public Task ServerSend(string message)
        {
            return Clients.All.SendAsync("ServerSend", message);
        }
    }
}
