using AetheriaWebService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AetheriaWebService.Models.Cell;

namespace AetheriaWebService.DataAccess
{
    public class AetheriaDataAccess
    {
        private AetheriaContext db;
        public AetheriaDataAccess(AetheriaContext context)
        {
            db = context;
        }
        public Player GetPlayer(string chatUserId, string platform)
        {
            var player = db.Players.Include(c => c.ChatUsers).FirstOrDefault(p => p.ChatUsers.Any(x => x.UserId == chatUserId && x.Platform == platform));
            return player;
        }
        public Cell GetCell(Entity entity)
        {
            Cell result = null;
            foreach (var cell in db.Cells)
            {
                var inventory = db.Inventories.Include(e => e.Entities).FirstOrDefault(x => x.InventoryId == cell.InventoryId);
                if (inventory.Entities != null && inventory.Entities.Any(x => x.EntityId == entity.EntityId))
                {
                    result = cell;
                }
            }
            //var cell = db.Cells.FirstOrDefault(x => x.Inventory.Entities.Any(y => y == entity));
            return result;
        }
        public Cell GetCellRelativeToCell(Cell cell, DirectionEnum direction)
        {
            Cell resultCell;
            float X = cell.X;
            float Y = cell.Y;
            float Z = cell.Z;
            switch (direction)
            {
                case DirectionEnum.North:
                    X++;
                    break;
                case DirectionEnum.South:
                    X--;
                    break;
                case DirectionEnum.East:
                    Y++;
                    break;
                case DirectionEnum.West:
                    Y--;
                    break;
                case DirectionEnum.Up:
                    Z++;
                    break;
                case DirectionEnum.Down:
                    Z--;
                    break;
            }

            resultCell = db.Cells.FirstOrDefault(c => c.X == X && c.Y == Y && c.Z == Z);

            return resultCell;
        }
        public void UpdateEntityCell(Entity entity, Cell newCell)
        {
            var oldCell = GetCell(entity);
            if (oldCell != null)
            {
                var oldInventory = oldCell.Inventory;
                oldInventory.Entities.Remove(entity);
                db.Update(oldInventory);
            }
            var newInventory = db.Inventories.Include(e => e.Entities).FirstOrDefault(x => x.InventoryId == newCell.InventoryId);
            if (newInventory.Entities == null)
            {
                newInventory.Entities = new List<Entity>();
            }
            newInventory.Entities.Add(entity);
            db.Update(newInventory);
            db.SaveChanges();
        }
        public Player CreateNewPlayer(string PlayerName, string Platform, string ChatUsername, string ChatUserId)
        {
            var chatUser = new ChatUser
            {
                ChatUserId = Guid.NewGuid(),
                Platform = Platform,
                Username = ChatUsername,
                UserId = ChatUserId,
                LastMessageDate = DateTime.Now
            };
            var chatUsers = new List<ChatUser>();
            chatUsers.Add(chatUser);
            var inventory = new Inventory
            {
                InventoryId = Guid.NewGuid(),
                Entities = new List<Entity>()
            };
            var dagger = new Weapon
            {
                EntityId = Guid.NewGuid(),
                Name = "Rusty Dagger",
                Description = "This is a rusty dagger, it sucks.",
                Type = Entity.EntityType.Weapon,
                DamageType = Weapon.EDamageType.Stabbing,
                BaseAPCost = 1,
                BaseDamageValue = 1,
                Effects = new List<Effect>(),
            };
            var characterStats = new CharacterStats
            {
                CharacterStatsId = Guid.NewGuid(),
                Level = 1,
                BaseMaximumActionPoints = 10d,
                BaseMaximumHealthPoints = 10d,
                CurrentActionPoints = 10d,
                CurrentHealthPoints = 10d,
                Effects = new List<Effect>()
            };
            var player = new Player
            {
                EntityId = Guid.NewGuid(),
                Name = PlayerName,
                Description = "How narcissistic of you",
                Type = Entity.EntityType.Player,
                WornEquipment = new List<Equipment>(),
                Stats = characterStats,
                ChatUsers = chatUsers,
                EquippedWeapon = dagger,
                Inventory = inventory
            };
            db.Players.Add(player);
            db.Weapons.Add(dagger);
            db.Inventories.Add(inventory);
            db.CharacterStats.Add(characterStats);
            db.ChatUsers.Add(chatUser);
            db.SaveChanges();
            var resultplayer = db.Players.FirstOrDefault(x => x.EntityId == player.EntityId);
            return resultplayer;
        }
        public World CreateNewWorld(string Name)
        {
            var world = new World
            {
                WorldId = Guid.NewGuid(),
                Name = Name,
                Cells = new List<Cell>()
            };

            db.Worlds.Add(world);
            db.SaveChanges();
            return world;
        }

        public Cell CreateNewCell(World World, int X, int Y, int Z, string Description, List<Entity> Entities)
        {
            //var world = db.Worlds.FirstOrDefault(x => x.WorldId == WorldId);
            var inventory = new Inventory
            {
                InventoryId = Guid.NewGuid(),
                Entities = Entities
            };
            var cell = new Cell
            {
                CellId = Guid.NewGuid(),
                Description = Description,
                Inventory = inventory,
                X = X,
                Y = Y,
                Z = Z,
                World = World
            };
            db.Cells.Add(cell);
            db.Inventories.Add(inventory);
            db.SaveChanges();
            return cell;
        }

        public List<ChatUser> GetRelevantChatUsersForPlayerAction(Player player)
        {
            var players = GetRelevantPlayersForPlayerAction(player);
            var chatUsers = RelevantChatUsers(players);
            return chatUsers;
        }

        public List<Player> GetRelevantPlayersForPlayerAction(Player player)
        {
            var playerCell = GetCell(player);
            var playerCellInventory = db.Inventories.Include(i => i.Entities).FirstOrDefault(x => x.InventoryId == playerCell.InventoryId);
            var playerEntities = playerCellInventory.Entities.Where(x => x.Type == Entity.EntityType.Player && x.EntityId != player.EntityId);
            var players = db.Players.Include(p => p.ChatUsers).Where(x => playerEntities.Any(y => y.EntityId == x.EntityId)).ToList();
            return players;
        }

        public List<ChatUser> RelevantChatUsers(List<Player> players)
        {
            var relevantUsers = new List<ChatUser>();

            foreach(var player in players)
            {
                relevantUsers.Add(player.ChatUsers.OrderByDescending(x => x.LastMessageDate).First());
            }
            
            return relevantUsers;
        }

        public string CellDescriptionForPlayer(Player player)
        {
            var description = "";

            var cell = GetCell(player);

            description += cell.Description;

            if (cell.Inventory.Entities.Count > 1)
            {
                description += " You can also see the following: ";
                var vowels = new string[5] { "a", "e", "i", "o", "u" };
                var descriptionItems = new List<string>();
                foreach (var entity in cell.Inventory.Entities)
                {
                    var startsWithVowel = vowels.Contains(entity.Name.Split()[0]);
                    if (entity.Type == Entity.EntityType.Player)
                    {
                        description += " " + entity.Name + ", ";
                    }
                    else
                    {
                        description += (startsWithVowel ? "an" : "a") + " " + entity.Name + ", ";
                    }
                }
                description = description.Substring(0, description.Length - 2);
            }

            return description;
        }
    }
}
