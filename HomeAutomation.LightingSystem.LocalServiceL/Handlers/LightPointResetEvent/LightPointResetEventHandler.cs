using HomeAutomation.LightingSystem.LocalServiceL.Mqtt;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Handlers.LightPointResetEvent
{
    public class LightPointResetEventHandler : AsyncRequestHandler<LightPointResetEvent>
    {
        private readonly IHomeAutomationMqttServer _mqttServer;

        public LightPointResetEventHandler(IHomeAutomationMqttServer mqttServer)
        {
            _mqttServer = mqttServer;
        }

        protected override async Task Handle(LightPointResetEvent request, CancellationToken cancellationToken)
        {
            await _mqttServer.PublishMessage("hardReset", request.LightPointId.ToString());
        }
    }
}
