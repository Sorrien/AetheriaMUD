using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AetheriaWebService.Models
{
    public class ChatUser
    {
        public Guid ChatUserId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Platform { get; set; }
        public DateTime LastMessageDate { get; set; }

        public Guid PlayerEntityId { get; set; }
    }
}