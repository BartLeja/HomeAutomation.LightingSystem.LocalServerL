using HomeAutomation.Core.Logger;
using MQTTnet.Server;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Mqtt
{
    public class ClientConnectedHandler : IMqttServerClientConnectedHandler
    {
        private ILokiLogger _lokiLogger;
        public ClientConnectedHandler(ILokiLogger lokiLogger)
        {
            _lokiLogger = lokiLogger;
        }
        public async Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            Debug.WriteLine($"Mqqt Client connected {eventArgs.ClientId}");
            await _lokiLogger.SendMessage($"Lighting System Mqqt Client connected {eventArgs.ClientId}");
        }
    }
}
