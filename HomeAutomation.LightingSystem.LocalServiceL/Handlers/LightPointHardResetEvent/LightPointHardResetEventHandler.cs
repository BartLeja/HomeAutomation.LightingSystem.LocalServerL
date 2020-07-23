using HomeAutomation.LightingSystem.LocalServiceL.Mqtt;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Handlers.LightPointHardResetEvent
{
    public class LightPointHardResetEventHandler : AsyncRequestHandler<LightPointHardResetEvent>
    {
        private readonly IHomeAutomationMqttServer _mqttServer;

        public LightPointHardResetEventHandler(IHomeAutomationMqttServer mqttServer)
        {
            _mqttServer = mqttServer;
        }

        protected override async Task Handle(LightPointHardResetEvent request, CancellationToken cancellationToken)
        {
            await _mqttServer.PublishMessage("reset", request.LightPointId.ToString());
        }
    }
}
