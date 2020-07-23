using MediatR;
using System;

namespace HomeAutomation.LightingSystem.LocalServiceL.Handlers.ReceiveLightPointEvent
{
    public class ReceiveLightPointEvent : IRequest
    {
        public Guid LightBulbId { get; set; }
        public bool Status { get; set; }

        public ReceiveLightPointEvent(Guid lightBulbId, bool status)
        {
            this.LightBulbId = lightBulbId;
            this.Status = status;
        }
    }
}
