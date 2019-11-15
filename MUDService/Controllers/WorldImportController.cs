using Microsoft.AspNetCore.Mvc;
using MUDService.DataAccess;
using MUDService.JsonModels;
using MUDService.Models;
using MUDService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MUDService.Controllers
{
    [Route("[controller]")]
    public class WorldImportController : Controller
    {
        private readonly MUDDataAccess _mudDataAccess;
        public WorldImportController(MUDDataAccess mudDataAccess)
        {
            _mudDataAccess = mudDataAccess;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = new WorldImportViewModel();
            model.WorldDefinition = new JWorld()
            {
                Name = "Test"
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Import(WorldImportViewModel request)
        {
            var world = await _mudDataAccess.CreateNewWorld(request.WorldDefinition.Name);
            foreach (var jCell in request.WorldDefinition.Cells)
            {
                var entities = new List<Entity>();

                foreach (var jEntity in jCell.Entities)
                {
                    var entityId = Guid.NewGuid();
                    switch (jEntity.Type)
                    {
                        case Entity.EntityType.Item:
                            var item = new Item()
                            {
                                EntityId = entityId,
                                Type = Entity.EntityType.Item,
                                Name = jEntity.Name,
                                Description = jEntity.Description,
                                Effects = new List<Effect>() //not going to worry about this yet                                
                            };
                            entities.Add(item);
                            break;
                        default:
                            var entity = new Entity()
                            {
                                EntityId = entityId,
                                Name = jEntity.Name,
                                Description = jEntity.Description,
                                Type = Entity.EntityType.None
                            };
                            entities.Add(entity);
                            break;
                    }
                }

                var cell = _mudDataAccess.CreateNewCell(world, jCell.X, jCell.Y, jCell.Z, jCell.Description, entities);
            }

            return View();
        }
    }
}