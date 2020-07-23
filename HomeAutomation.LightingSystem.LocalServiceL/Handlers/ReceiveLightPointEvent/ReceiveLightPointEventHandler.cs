using HomeAutomation.LightingSystem.LocalServiceL.Models;
using HomeAutomation.LightingSystem.LocalServiceL.Mqtt;
using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Handlers.ReceiveLightPointEvent
{
    public class ReceiveLightPointEventHandler : AsyncRequestHandler<ReceiveLightPointEvent>
    {
        private readonly IHomeAutomationMqttServer _mqttServer;

        public ReceiveLightPointEventHandler(IHomeAutomationMqttServer mqttServer)
        {
            _mqttServer = mqttServer;
        }

        protected override async Task Handle(ReceiveLightPointEvent request, CancellationToken cancellationToken)
        {
            var lightPointStatus = new LightPointStatus()
            {
                Id = request.LightBulbId.ToString(),
                Status = request.Status
            };

            string lightPointStatusPayload = JsonConvert.SerializeObject(lightPointStatus);

            await _mqttServer.PublishMessage("lightChange", lightPointStatusPayload);
        }
    }
}
