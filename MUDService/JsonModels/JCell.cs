using System.Collections.Generic;

namespace MUDService.JsonModels
{
    public class JCell
    {
        public string Description { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public List<JEntity> Entities { get; set; }
    }
}