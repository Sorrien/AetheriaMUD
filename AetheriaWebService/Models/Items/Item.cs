using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDService.Models
{
    public class Item : Entity
    {

        public List<Effect> Effects { get; set; }
    }
}
