using HomeAutomation.LightingSystem.LocalServiceL.Models;
using HomeAutomation.LightingSystem.LocalServiceL.Mqtt;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Handlers.ReceiveLightsPointGroupEvent
{
    public class ReceiveLightsPointGroupEventHandler : AsyncRequestHandler<ReceiveLightsPointGroupEvent>
    {
        private readonly IHomeAutomationMqttServer _mqttServer;

        public ReceiveLightsPointGroupEventHandler(IHomeAutomationMqttServer mqttServer)
        {
            _mqttServer = mqttServer;
        }

        protected override async Task Handle(ReceiveLightsPointGroupEvent request, CancellationToken cancellationToken)
        {
            request.LightBulbsId.ToList().ForEach(async lp =>
            {
                var lightPointStatusPayload = JsonConvert.SerializeObject(

                        new LightPointStatus()
                        {
                            Id = lp.ToString(),
                            Status = request.Status
                        }
                    );

                await _mqttServer.PublishMessage("lightChange", lightPointStatusPayload);
            });
        }
    }
}
