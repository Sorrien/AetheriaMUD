using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDService.Models
{
    public class Character : Entity
    {
        public CharacterStats Stats { get; set; }
        public List<Equipment> WornEquipment { get; set; }
        public Weapon EquippedWeapon { get; set; }
        public Inventory Inventory { get; set; }
    }
}
