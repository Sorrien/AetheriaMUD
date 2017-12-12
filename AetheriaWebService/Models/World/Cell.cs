using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AetheriaWebService.Models
{
    public class Cell
    {
        public enum DirectionEnum
        {
            None = 0,
            North = 10,
            South = 20,
            West = 30,
            East = 40,
            Up = 50,
            Down = 60
        }
        public Guid CellId { get; set; }
        
        public string Description { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Guid InventoryId { get; set; }
        public Inventory Inventory { get; set; }
        public World World { get; set; }

        public string EntitiesDescription { get
            {
                var description = "You can also see the following: ";
                var vowels = new string[5] { "a", "e", "i", "o", "u"};
                foreach (var entity in Inventory.Entities)
                {
                    var startsWithVowel = vowels.Contains(entity.Name.Split()[0]);
                    description += (startsWithVowel ? "an" : "a") + " " + entity.Name + ",";
                }
                description = description.Substring(0, description.Length - 1);
                return description;
            }
        }
    }
}
