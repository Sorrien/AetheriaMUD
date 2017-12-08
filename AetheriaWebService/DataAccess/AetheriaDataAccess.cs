using AetheriaWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AetheriaWebService.Models.Cell;

namespace AetheriaWebService.DataAccess
{
    public class AetheriaDataAccess
    {
        private AetheriaContext db;
        public AetheriaDataAccess(AetheriaContext context)
        {
            db = context;
        }
        public Cell GetCell(Entity entity)
        {
            var cell = db.Cells.FirstOrDefault(x => x.Inventory.InventoryId == entity.Inventory.InventoryId);
            return cell;
        }
        public Cell GetCellRelativeToCell(Cell cell, DirectionEnum direction)
        {
            Cell resultCell;
            float X = cell.X;
            float Y = cell.Y;
            float Z = cell.Z;
            switch (direction)
            {
                case DirectionEnum.North:
                    X++;
                    break;
                case DirectionEnum.South:
                    X--;
                    break;
                case DirectionEnum.East:
                    Y++;
                    break;
                case DirectionEnum.West:
                    Y--;
                    break;
                case DirectionEnum.Up:
                    Z++;
                    break;
                case DirectionEnum.Down:
                    Z--;
                    break;
            }

            resultCell = db.Cells.FirstOrDefault(c => c.X == X && c.Y == Y && c.Z == Z);

            return resultCell;
        }
        public void UpdateEntityCell(Entity entity, Cell newCell)
        {
            var oldCell = db.Cells.FirstOrDefault(x => x.Inventory.Entities.Contains(entity));
            oldCell.Inventory.Entities.Remove(entity);
            db.Update(oldCell);
            newCell.Inventory.Entities.Add(entity);
            db.Update(newCell);
            db.SaveChanges();
        }
    }
}
