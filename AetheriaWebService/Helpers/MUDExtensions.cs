using MUDService.DataAccess;
using MUDService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDService.Helpers
{
    public static class MUDExtensions
    {
        public static void EnsureSeedData(this MUDContext context)
        {
            var mudDataAccess = new MUDDataAccess(context);
            if (!context.Database.GetPendingMigrations().Any())
            {
                if (!context.Cells.Any())
                {
                    var world = mudDataAccess.CreateNewWorld("Test");

                    //int x = 0;
                    //int y = 0;
                    string description = "Rolling grassy hills stretch out as far as you can see.";
                    var entities = new List<Entity>();
                    int z = 0;
                    for(int i=0;i<3;i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            mudDataAccess.CreateNewCell(world, i, j, z, description, entities);
                        }
                    }
                }

                //if (!context.Players.Any())
                //{
                //var player = mudDataAccess.CreateNewPlayer("Sorrien", "slack", "collinsparks", "U6G34P0S1");
                //var player2 = mudDataAccess.CreateNewPlayer("Mavrick", "slack", "connor", "U6P6XEATG");
                //var player3 = mudDataAccess.CreateNewPlayer("Pasha", "slack", "pasha", "U7DDTDB7F");
                //var startingCell = context.Cells.FirstOrDefault(x => x.X == 0 && x.Y == 0 && x.Z == 0);
                //mudDataAccess.UpdateEntityCell(player, startingCell);
                //mudDataAccess.UpdateEntityCell(player2, startingCell);
                //mudDataAccess.UpdateEntityCell(player3, startingCell);
                //}
            }
        }
    }
}
