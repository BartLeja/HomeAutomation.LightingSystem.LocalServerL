using HomeAutomation.LightingSystem.LocalServiceL.Clients;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Handlers.SwitchLightPointEvent
{
    public class SwitchLightPointEventHandler : AsyncRequestHandler<SwitchLightPointEvent>
    {
        //private readonly ISignalRClient _signalRClient;

        public SwitchLightPointEventHandler(
           // ISignalRClient signalRClient
            )
        {
            //_signalRClient = signalRClient;
        }

        protected override async Task Handle(SwitchLightPointEvent request, CancellationToken cancellationToken)
        {
           // await _signalRClient.InvokeSendStatusMethod(request.LightPointId, request.LightPointStatus);
        }
    }
}
