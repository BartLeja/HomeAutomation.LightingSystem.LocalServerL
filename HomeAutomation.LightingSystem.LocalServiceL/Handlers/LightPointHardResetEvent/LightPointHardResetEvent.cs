using MediatR;
using System;

namespace HomeAutomation.LightingSystem.LocalServiceL.Handlers.LightPointHardResetEvent
{
    public class LightPointHardResetEvent : IRequest
    {
        public Guid LightPointId { get; set; }

        public LightPointHardResetEvent(Guid lightPointId)
        {
            LightPointId = lightPointId;
        }
    }
}
