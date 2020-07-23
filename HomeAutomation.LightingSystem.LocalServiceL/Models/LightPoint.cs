using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Models
{
    public class LightPoint
    {
        public string Id { get; set; }
        public string CustomName { get; set; }

        public string[] BulbsId { get; set; }
    }
}
