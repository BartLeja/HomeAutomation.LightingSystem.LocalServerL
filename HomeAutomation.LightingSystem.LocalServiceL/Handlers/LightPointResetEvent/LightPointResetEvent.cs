using MediatR;
using System;

namespace HomeAutomation.LightingSystem.LocalServiceL.Handlers.LightPointResetEvent
{
    public class LightPointResetEvent : IRequest
    {
        public Guid LightPointId { get; set; }

        public LightPointResetEvent(Guid lightPointId)
        {
            LightPointId = lightPointId;
        }
    }
}
