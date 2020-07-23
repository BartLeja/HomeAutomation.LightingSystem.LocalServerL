using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Dto
{
    public class LightPointDto
    {
        public Guid Id { get; set; }
        public string CustomName { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<LightBulbDto> LightBulbs { get; set; }
    }
}
