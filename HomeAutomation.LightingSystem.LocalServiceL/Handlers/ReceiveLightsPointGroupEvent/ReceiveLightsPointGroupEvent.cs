using MediatR;
using System;
using System.Collections.Generic;

namespace HomeAutomation.LightingSystem.LocalServiceL.Handlers.ReceiveLightsPointGroupEvent
{
    public class ReceiveLightsPointGroupEvent : IRequest
    {
        public IEnumerable<Guid> LightBulbsId { get; set; }
        public bool Status { get; set; }

        public ReceiveLightsPointGroupEvent(IEnumerable<Guid> lightBulbsId, bool status)
        {
            this.LightBulbsId = lightBulbsId;
            this.Status = status;
        }
    }
}
