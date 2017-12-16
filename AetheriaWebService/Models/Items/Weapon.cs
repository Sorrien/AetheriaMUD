using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDService.Models
{
    public class Weapon : Item
    {

        public enum EDamageType
        {
            None = 0,
            Slashing = 10,
            Stabbing = 20,
            Blunt = 30,
        }
        public EDamageType DamageType { get; set; }
        public double BaseDamageValue {get;set;}
        public double BaseAPCost { get; set; }
    }
}
