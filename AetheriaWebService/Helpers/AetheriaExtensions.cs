using AetheriaWebService.DataAccess;
using AetheriaWebService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AetheriaWebService.Helpers
{
    public static class AetheriaExtensions
    {
        public static void EnsureSeedData(this AetheriaContext context)
        {
            var aetheriaDataAccess = new AetheriaDataAccess(context);
            if (!context.Database.GetPendingMigrations().Any())
            {
                if (!context.Cells.Any())
                {
                    var world = aetheriaDataAccess.CreateNewWorld("Test");

                    //int x = 0;
                    //int y = 0;
                    string description = "Rolling grassy hills stretch out as far as you can see.";
                    var entities = new List<Entity>();
                    int z = 0;
                    for(int i=0;i<3;i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            aetheriaDataAccess.CreateNewCell(world, i, j, z, description, entities);
                        }
                    }
                }

                if (!context.Players.Any())
                {
                    var player = aetheriaDataAccess.CreateNewPlayer("Sorrien", "slack", "collinsparks", "U6G34P0S1");
                    var player2 = aetheriaDataAccess.CreateNewPlayer("Mavrick", "slack", "connor", "U6P6XEATG");
                    var startingCell = context.Cells.FirstOrDefault(x => x.X == 0 && x.Y == 0 && x.Z == 0);
                    aetheriaDataAccess.UpdateEntityCell(player, startingCell);
                    aetheriaDataAccess.UpdateEntityCell(player2, startingCell);
                }
            }
        }
    }
}
