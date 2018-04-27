using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDService.Models
{
    public class Inventory
    {
        public Guid InventoryId { get; set; }
        public List<Entity> Entities { get; set; }
    }
}
