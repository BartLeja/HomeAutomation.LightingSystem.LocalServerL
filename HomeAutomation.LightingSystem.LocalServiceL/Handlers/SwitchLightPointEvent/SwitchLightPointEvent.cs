using MediatR;
using System;

namespace HomeAutomation.LightingSystem.LocalServiceL.Handlers.SwitchLightPointEvent
{
    public class SwitchLightPointEvent : IRequest 
    {
        public Guid LightPointId { get; set; }
        public bool LightPointStatus { get; set; }

        public SwitchLightPointEvent(Guid lightPointId, bool lightPointStatus)
        {
            this.LightPointId = lightPointId;
            this.LightPointStatus = lightPointStatus;
        }
    }
}
