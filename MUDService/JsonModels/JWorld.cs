using System.Collections.Generic;

namespace MUDService.JsonModels
{
    public class JWorld
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<JCell> Cells { get; set; }
    }
}
