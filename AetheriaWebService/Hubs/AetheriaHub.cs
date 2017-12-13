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
        private AetheriaContext db;
        // private ReplicationHelper _replicationHelper;
        private IHubContext<AetheriaHub> _messageHubContext;
        public AetheriaHub(AetheriaContext context,
            //ReplicationHelper replicationHelper
            IHubContext<AetheriaHub> messageHubContext
            )
        {
            db = context;
            _messageHubContext = messageHubContext;
            //_replicationHelper = replicationHelper;
        }
        public Task Send(string message)
        {
            var clientMessage = JsonConvert.DeserializeObject<AetheriaClientMessage>(message);
            var replicationHelper = new ReplicationHelper(_messageHubContext);
            var aetheriaDataAccess = new AetheriaDataAccess(db);
            var aetheriaHelper = new AetheriaHelper(aetheriaDataAccess, replicationHelper);
            var player = aetheriaDataAccess.GetPlayer(clientMessage.ChatUserId, clientMessage.Platform);
            var response = aetheriaHelper.ProcessPlayerInput(clientMessage.Message, player);
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

        public Task ServerSend(string message)
        {
            return Clients.All.InvokeAsync("ServerSend", message);
        }
    }

    public class ReplicationHelper
    {
        private IHubContext<AetheriaHub> _messageHubContext;

        public ReplicationHelper(IHubContext<AetheriaHub> messageHubContext)
        {
            _messageHubContext = messageHubContext;
        }


        public async void ReplicateToClients(string message, List<ChatUser> chatUsers)
        {
            var relevantChatUsers = new List<ChatUserDTO>();
            foreach (var chatUser in chatUsers)
            {
                relevantChatUsers.Add(new ChatUserDTO
                {
                    ChatUserId = chatUser.UserId,
                    Platform = chatUser.Platform
                });
            }
            var serverResponse = new AetheriaServerResponse
            {
                ServerAuthToken = "TestToken",
                RelevantChatUsers = relevantChatUsers,
                Response = message
            };
            var responseMessage = JsonConvert.SerializeObject(serverResponse);

            await _messageHubContext.Clients.All.InvokeAsync("Send", responseMessage);
        }
    }
}
