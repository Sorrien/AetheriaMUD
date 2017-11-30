using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AetheriaWebService.Models
{
    public class Cell
    {
        public Guid CellId { get; set; }
        
        public string Description { get; set; }

        public Position Position { get; set; }
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
