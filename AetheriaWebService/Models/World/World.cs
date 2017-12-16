using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDService.Models
{
    public class World
    {
        public Guid WorldId { get; set; }

        public string Name { get; set; }
        public List<Cell> Cells { get; set; }
    }
}
