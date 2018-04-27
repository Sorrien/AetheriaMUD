using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDService.JsonModels
{
    public class JWorld
    {
        public string Name { get; set; }
        public List<JCell> Cells { get; set; }
    }
}
