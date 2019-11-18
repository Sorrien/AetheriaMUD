using System;

namespace MUDService.Models
{
    public class Cell
    {
        public Guid CellId { get; set; }

        public string Description { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Guid InventoryId { get; set; }
        public Inventory Inventory { get; set; }
        public World World { get; set; }
    }
}
