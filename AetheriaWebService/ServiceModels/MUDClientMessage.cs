using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDService.ServiceModels
{
    public class MUDClientMessage
    {
        public string ClientId { get; set; }
        public string Platform { get; set; }
        public string ChatUsername { get; set; }
        public string ChatUserId { get; set; }
        public string Message { get; set; }
    }
}
