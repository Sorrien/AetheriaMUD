using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MUDService.JsonModels;
using MUDService.ViewModels;
using Newtonsoft.Json;

namespace MUDService.Controllers
{
    [Route("[controller]")]
    public class WorldImportController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new WorldImportViewModel();
            model.WorldDefinition = "{\"Name\" : \"Test\"}";
            var jWorld = JsonConvert.DeserializeObject<JWorld>(model.WorldDefinition);
            return View(model);
        }

        [HttpPost]
        public IActionResult Import(WorldImportViewModel viewModel)
        {
            var jsonString = viewModel.WorldDefinition;

            var jWorld = JsonConvert.DeserializeObject<JWorld>(jsonString);
            return null;
        }
    }
}