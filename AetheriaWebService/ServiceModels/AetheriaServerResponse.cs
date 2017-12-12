using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AetheriaWebService.ServiceModels
{
    public class AetheriaServerResponse
    {
        public string ServerAuthToken { get; set; }
        public string Platform { get; set; }
        public string ChatUserId { get; set; }
        public string Response { get; set; }
    }
}
