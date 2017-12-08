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
            if (!context.Database.GetPendingMigrations().Any())
            {
                if (!context.Cells.Any())
                {
                    var world = new World
                    {
                        WorldId = Guid.NewGuid(),
                        Name = "Test"
                    };
                    context.Worlds.Add(world);

                    //int x = 0;
                    //int y = 0;
                    int z = 0;
                    for(int i=0;i<3;i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            var inventory = new Inventory
                            {
                                InventoryId = Guid.NewGuid(),
                                Entities = new List<Entity>()
                            };
                            var cell = new Cell
                            {
                                CellId = Guid.NewGuid(),
                                Description = "a tile of world stretches out before you",
                                Inventory = inventory,
                                X = i,
                                Y = j,
                                Z = z,
                                World = world
                            };
                        }

                    }

                    context.SaveChanges();
                }

                if (!context.Players.Any())
                {
                    var player = new Player
                    {
                        EntityId = Guid.NewGuid(),
                        Name = "Sorrien",
                        Description = "the great necromancer",
                        Type = Entity.EntityType.Player,
                        WornEquipment = new List<Equipment>(),
                        Stats = new CharacterStats
                        {
                            CharacterStatsId = Guid.NewGuid(),
                            Level = 1,
                            BaseMaximumActionPoints = 10d,
                            BaseMaximumHealthPoints = 10d,
                            CurrentActionPoints = 10d,
                            CurrentHealthPoints = 10d,
                            Effects = new List<Effect>()
                        },
                        ChatUsers = new List<ChatUser>()
                    };
                    player.ChatUsers.Add(new ChatUser
                    {
                        ChatUserId = Guid.NewGuid(),
                        Platform = "slack",
                        Username = "csparks",
                        UserId = "utest"
                    });
                    context.Players.Add(player);
                    var startingCell = context.Cells.FirstOrDefault(x => x.X == 0 && x.Y == 0 && x.Z == 0);
                    startingCell.Inventory.Entities.Add(player);
                    context.Update(startingCell);
                    context.SaveChanges();
                }
            }
        }
    }
}
