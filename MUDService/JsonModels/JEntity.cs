using static MUDService.Models.Entity;

namespace MUDService.JsonModels
{
    public class JEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EntityType Type { get; set; }
    }
}