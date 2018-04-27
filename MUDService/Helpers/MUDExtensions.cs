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
                //}
            }
        }
    }
}
