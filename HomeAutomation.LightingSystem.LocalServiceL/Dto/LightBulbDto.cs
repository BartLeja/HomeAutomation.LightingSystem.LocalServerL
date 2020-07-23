using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Dto
{
    public class LightBulbDto
    {
        public Guid Id { get; set; }
        public bool Status { get; set; } = false;
    }
}
