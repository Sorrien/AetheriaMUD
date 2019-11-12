using System;

namespace MUDService.Models
{
    public class Entity
    {
        public enum EntityType
        {
            None = 0,
            NPC = 10,
            Player = 20,
            Consumable = 30,
            Item = 40,
            Weapon = 50,
            Equipment = 60,
        }
        public Guid EntityId { get; set; }
        public EntityType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}