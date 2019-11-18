using MUDService.DataAccess;
using MUDService.Helpers;
using MUDService.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MUDService.Models.Cell;

namespace MUDService.Logic
{
    public interface ICellLogic
    {
        Task<Cell> GetPlayerCell(Player player);
        Task<string> CellDescriptionForPlayer(Player player);
        Task<Cell> GetCellRelativeToCell(Cell cell, DirectionEnum direction);
        Task<Cell> GetCellInWorld(string worldName, int x, int y, int z);
        Task UpdateEntityCell(Entity entity, Cell newCell);
    }

    public class CellLogic : ICellLogic
    {
        private readonly IMUDDataAccess _mudDataAccess;
        public CellLogic(IMUDDataAccess mudDataAccess)
        {
            _mudDataAccess = mudDataAccess;
        }

        public async Task<Cell> GetPlayerCell(Player player)
        {
            return await _mudDataAccess.GetCell(player);
        }

        public async Task<string> CellDescriptionForPlayer(Player player)
        {
            var cell = await GetPlayerCell(player);
            return CellDescription(cell, player);
        }

        public string CellDescription(Cell cell, Entity exclude)
        {
            var stringBuilder = new StringBuilder(cell.Description);

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

        public async Task<Cell> GetCellRelativeToCell(Cell cell, DirectionEnum direction)
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

            resultCell = await _mudDataAccess.GetCellInWorld(cell.World.Name, X, Y, Z);

            return resultCell;
        }

        public async Task<Cell> GetCellInWorld(string worldName, int x, int y, int z)
        {
            return await _mudDataAccess.GetCellInWorld(worldName, x, y, z);
        }

        public async Task UpdateEntityCell(Entity entity, Cell newCell)
        {
            await _mudDataAccess.UpdateEntityCell(entity, newCell);
        }
    }
}
