using System.Collections.Generic;

namespace MUDService.Models
{
    public class Player : Character
    {
        public List<ChatUser> ChatUsers { get; set; }
    }
}