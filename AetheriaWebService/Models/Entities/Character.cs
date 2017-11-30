using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AetheriaWebService.Models
{
    public class Character : Entity
    {
        public CharacterStats Stats { get; set; }
        public List<Equipment> WornEquipment { get; set; }
        public Weapon EquippedWeapon { get; set; }
        public List<Item> Inventory { get; set; }
    }
}
