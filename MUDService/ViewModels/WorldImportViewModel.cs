using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDService.ViewModels
{
    
    public class WorldImportViewModel
    {
        [FromForm]
        public string WorldDefinition { get; set; }
    }
}
