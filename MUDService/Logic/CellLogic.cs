using MUDService.DataAccess;
using MUDService.Helpers;
using MUDService.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            var cell = GetPlayerCell(player);
            return CellDescription(cell, player);
        }

        public string CellDescription(Cell cell, Entity exclude)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(cell.Description);
            var validEntities = cell.Inventory.Entities.Where(x => x.EntityId != exclude.EntityId).ToList();

            if (validEntities.Count > 1)
            {
                stringBuilder.Append(" You can also see the following: ");
            }
            else if (validEntities.Count == 1)
            {
                stringBuilder.Append(" You can also see ");
            }

            stringBuilder.DescriptionList(validEntities.Select(x => x.Name).ToList());

            stringBuilder.Append(".");
            var description = stringBuilder.ToString();
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
