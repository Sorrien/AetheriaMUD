using MUDService.DataAccess;
using MUDService.Helpers;
using MUDService.Models;
using System.Linq;
using static MUDService.Models.Cell;

namespace MUDService.Logic
{
    public interface ICellLogic
    {
        Cell GetPlayerCell(Player player);
        string CellDescriptionForPlayer(Player player);
        Cell GetCellRelativeToCell(Cell cell, DirectionEnum direction);
        Cell GetCellInWorld(string worldName, int x, int y, int z);
        void UpdateEntityCell(Entity entity, Cell newCell);
    }

    public class CellLogic : ICellLogic
    {
        private readonly IMUDDataAccess _mudDataAccess;
        public CellLogic(IMUDDataAccess mudDataAccess)
        {
            _mudDataAccess = mudDataAccess;
        }

        public Cell GetPlayerCell(Player player)
        {
            return _mudDataAccess.GetCell(player);
        }

        public string CellDescriptionForPlayer(Player player)
        {
            var description = "";

            var cell = _mudDataAccess.GetCell(player);

            description += cell.Description;

            if (cell.Inventory.Entities.Count > 1)
            {
                description += " You can also see the following: ";
                foreach (var entity in cell.Inventory.Entities.Where(x => x.EntityId != player.EntityId))
                {
                    if (entity.Type == Entity.EntityType.Player)
                    {
                        description += $" {entity.Name}, ";
                    }
                    else
                    {
                        description += $"{entity.Name.GetAOrAnFromInput()} {entity.Name}, ";
                    }
                }
                description = description.Substring(0, description.Length - 2);
            }

            return description;
        }

        public Cell GetCellRelativeToCell(Cell cell, DirectionEnum direction)
        {
            Cell resultCell;
            var X = cell.X;
            var Y = cell.Y;
            var Z = cell.Z;
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

            resultCell = _mudDataAccess.GetCellInWorld(cell.World.Name, X, Y, Z);

            return resultCell;
        }

        public Cell GetCellInWorld(string worldName, int x, int y, int z)
        {
            return _mudDataAccess.GetCellInWorld(worldName, x, y, z);
        }

        public void UpdateEntityCell(Entity entity, Cell newCell)
        {
            _mudDataAccess.UpdateEntityCell(entity, newCell);
        }
    }
}
