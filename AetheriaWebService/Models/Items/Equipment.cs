using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MUDService.Models.Weapon;

namespace MUDService.Models
{
    public class Equipment : Item
    {
        public enum ESlotType
        {
            None = 0,
            Head = 10,
            Chest = 20,
            Legs = 30,
            Feet = 40,
            Hands = 50,
            Neck = 60,
            Ring = 70,
        }

        public ESlotType SlotType { get; set; }

        public EDamageType ResistType { get; set; }

        public double ResistAmount { get; set; }
    }
}
