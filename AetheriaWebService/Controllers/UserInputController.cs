using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AetheriaWebService.Helpers;
using AetheriaWebService.Models;

namespace AetheriaWebService.Controllers
{
    [Produces("application/json")]
    [Route("api/UserInput")]
    public class UserInputController : Controller
    {
        // GET api/values/5
        [HttpGet("{input}")]
        public string Get(string input, string chatUsername)
        {
            var aetheriaHelper = new AetheriaHelper();
            Player player = aetheriaHelper.GetPlayer(chatUsername);
            string response = aetheriaHelper.ProcessPlayerInput(input, player);
            return response;
        }
    }
}