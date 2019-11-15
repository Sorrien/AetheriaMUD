using Microsoft.EntityFrameworkCore;
using MUDService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MUDService.Models.Cell;

namespace MUDService.DataAccess
{
    public interface IMUDDataAccess
    {
        Task<Player> GetPlayer(string chatUserId, string platform);
        Task<Cell> GetCell(Entity entity);
        Task UpdateEntityInventory(Inventory old, Inventory newInventory, Entity entity);
        Task UpdateEntityCell(Entity entity, Cell newCell);
        Task<Player> CreateNewPlayer(string PlayerName, string Platform, string ChatUsername, string ChatUserId);
        Task<World> CreateNewWorld(string Name);
        Task<Cell> CreateNewCell(World World, int X, int Y, int Z, string Description, List<Entity> Entities);
        Task<List<ChatUser>> GetRelevantChatUsersForPlayerAction(Player player);
        Task<Cell> GetCellInWorld(string worldName, int x, int y, int z);
        Task RenameEntity(string NewName, Entity entity);
        Task UpdatePlayerIsMuted(Player player, bool IsMuted);
    }
    public class MUDDataAccess : IMUDDataAccess
    {
        private readonly MUDContext db;
        public MUDDataAccess(MUDContext context)
        {
            db = context;
        }
        public async Task<Player> GetPlayer(string chatUserId, string platform)
        {
            var player = await db.Players.Include(c => c.ChatUsers).Include(x => x.Inventory).ThenInclude(x => x.Entities).Include(x => x.EquippedWeapon).Include(x => x.WornEquipment).FirstOrDefaultAsync(p => p.ChatUsers.Any(x => x.UserId == chatUserId && x.Platform == platform));
            //player.Inventory = db.Inventories.Include(x => x.Entities).FirstOrDefault(x => x.InventoryId == player.Inventory.InventoryId);
            return player;
        }
        public async Task<List<ChatUser>> GetChatUsersForPlayer(Player player)
        {
            var chatUsers = await db.ChatUsers.Where(x => x.PlayerEntityId == player.EntityId).ToListAsync();
            return chatUsers;
        }
        public async Task<Cell> GetCellInWorld(string worldName, int x, int y, int z)
        {
            var world = await db.Worlds.Include(c => c.Cells).FirstOrDefaultAsync(w => w.Name == worldName);
            var cell = world.Cells.FirstOrDefault(c => c.X == x && c.Y == y && c.Z == z);
            return cell;
        }
        public async Task<Cell> GetCell(Entity entity)
        {
            var cell = await db.Cells.Include(x => x.Inventory).ThenInclude(x => x.Entities).FirstOrDefaultAsync(x => x.Inventory.Entities.Any(y => y == entity));
            return cell;
        }
        public async Task UpdateEntityInventory(Inventory old, Inventory newInventory, Entity entity)
        {
            old.Entities.Remove(entity);
            newInventory.Entities.Add(entity);
            db.Update(old);
            db.Update(newInventory);
            await db.SaveChangesAsync();
        }
        public async Task UpdateEntityCell(Entity entity, Cell newCell)
        {
            var oldCell = await GetCell(entity);
            if (oldCell != null)
            {
                var oldInventory = oldCell.Inventory;
                oldInventory.Entities.Remove(entity);
                db.Update(oldInventory);
            }
            var newInventory = await db.Inventories.Include(e => e.Entities).FirstOrDefaultAsync(x => x.InventoryId == newCell.InventoryId);
            if (newInventory.Entities == null)
            {
                newInventory.Entities = new List<Entity>();
            }
            newInventory.Entities.Add(entity);
            db.Update(newInventory);
            await db.SaveChangesAsync();
        }
        public async Task<Player> CreateNewPlayer(string PlayerName, string Platform, string ChatUsername, string ChatUserId)
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
            var goldbar = new Entity
            {
                EntityId = Guid.NewGuid(),
                Type = Entity.EntityType.Item,
                Name = "gold bar",
                Description = "an ingot of gold, quite valuable."
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
            inventory.Entities.Add(goldbar);
            db.Entities.Add(goldbar);
            db.Players.Add(player);
            db.Weapons.Add(dagger);
            db.Inventories.Add(inventory);
            db.CharacterStats.Add(characterStats);
            db.ChatUsers.Add(chatUser);
            db.SaveChanges();

            var startingCell = await db.Cells.FirstOrDefaultAsync(x => x.X == 0 && x.Y == 0 && x.Z == 0);
            await UpdateEntityCell(player, startingCell);
            await db.SaveChangesAsync();

            var resultplayer = await db.Players.FirstOrDefaultAsync(x => x.EntityId == player.EntityId);
            return resultplayer;
        }
        public async Task<World> CreateNewWorld(string Name)
        {
            var world = new World
            {
                WorldId = Guid.NewGuid(),
                Name = Name,
                Cells = new List<Cell>()
            };

            db.Worlds.Add(world);
            await db.SaveChangesAsync();
            return world;
        }

        public async Task<Cell> CreateNewCell(World World, int X, int Y, int Z, string Description, List<Entity> Entities)
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
            await db.SaveChangesAsync();
            return cell;
        }

        public async Task<List<ChatUser>> GetRelevantChatUsersForPlayerAction(Player player)
        {
            var playerCell = await GetCell(player);
            var playerCellInventory = await db.Inventories.Include(i => i.Entities).FirstOrDefaultAsync(x => x.InventoryId == playerCell.InventoryId);
            var playerEntities = playerCellInventory.Entities.Where(x => x.Type == Entity.EntityType.Player && x.EntityId != player.EntityId);
            var chatUsers = await db.ChatUsers.Where(x => playerEntities.Any(y => y.EntityId == x.PlayerEntityId && !x.IsMuted)).ToListAsync();
            return chatUsers;
        }

        public async Task RenameEntity(string NewName, Entity entity)
        {
            entity.Name = NewName;
            db.Update(entity);
            await db.SaveChangesAsync();
        }

        public async Task UpdatePlayerIsMuted(Player player, bool IsMuted)
        {
            var chatUsers = await GetChatUsersForPlayer(player);
            foreach(var chatUser in chatUsers)
            {
                chatUser.IsMuted = IsMuted;
            }
            db.ChatUsers.UpdateRange(chatUsers);
            await db.SaveChangesAsync();
        }
    }
}
