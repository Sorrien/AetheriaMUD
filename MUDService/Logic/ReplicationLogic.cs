using Microsoft.AspNetCore.SignalR;
using MUDService.DataAccess;
using MUDService.Hubs;
using MUDService.Models;
using MUDService.ServiceModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MUDService.Logic
{
    public interface IReplicationLogic
    {
        Task ReplicatePlayerAction(string message, Player player);
        Task ReplicateToClients(string message, List<ChatUser> chatUsers);
    }
    public class ReplicationLogic : IReplicationLogic
    {
        private readonly IHubContext<MUDHub> _messageHubContext;
        private readonly IMUDDataAccess _mudDataAccess;

        public ReplicationLogic(IHubContext<MUDHub> messageHubContext, IMUDDataAccess mudDataAccess)
        {
            _messageHubContext = messageHubContext;
            _mudDataAccess = mudDataAccess;
        }

        public async Task ReplicatePlayerAction(string message, Player player)
        {
            var chatUsers = _mudDataAccess.GetRelevantChatUsersForPlayerAction(player);
            if (chatUsers.Count > 0)
            {
                await ReplicateToClients(message, chatUsers);
            }
        }

        public async Task ReplicateToClients(string message, List<ChatUser> chatUsers)
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
            var serverResponse = new MUDServerResponse
            {
                ServerAuthToken = "TestToken",
                RelevantChatUsers = relevantChatUsers,
                Response = message
            };
            var responseMessage = JsonConvert.SerializeObject(serverResponse);

            await _messageHubContext.Clients.All.SendAsync("Send", responseMessage);
        }
    }
}