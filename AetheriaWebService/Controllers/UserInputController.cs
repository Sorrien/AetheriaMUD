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
        private AetheriaContext db;
        public UserInputController(AetheriaContext context)
        {
            db = context;
        }
        // GET api/values/5
        [HttpGet("{input}, {chatUsername}")]
        public string Get(string input, string chatUsername)
        {
            var aetheriaDataAccess = new DataAccess.AetheriaDataAccess(db);
            var aetheriaHelper = new AetheriaHelper(aetheriaDataAccess);
            Player player = aetheriaHelper.GetPlayer(chatUsername);
            string response = aetheriaHelper.ProcessPlayerInput(input, player);
            return response;
        }
    }
}