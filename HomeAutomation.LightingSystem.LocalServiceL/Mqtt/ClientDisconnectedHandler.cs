using HomeAutomation.Core.Logger;
using HomeAutomation.LightingSystem.LocalServiceL.Clients;
using MQTTnet.Server;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Mqtt
{
    public class ClientDisconnectedHandler : IMqttServerClientDisconnectedHandler
    {
        private readonly IRestClient _restClient;
        private readonly IMqttServer _mqttServer;
        private ILokiLogger _lokiLogger;

        public ClientDisconnectedHandler(IRestClient restClient, IMqttServer mqttServer, ILokiLogger lokiLogger)
        {
            _restClient = restClient;
            _mqttServer = mqttServer;
            _lokiLogger = lokiLogger;
        }

        public async Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            Debug.WriteLine($"Client disconnected {eventArgs.ClientId}");
            await _mqttServer.ClearRetainedApplicationMessagesAsync();
            try
            {
                await _restClient.DisableLightPoint(Guid.Parse(eventArgs.ClientId));
            }
            catch (Exception ex)
            {
                await _lokiLogger.SendMessage($"Lighting System {ex}", LogLevel.Error);
                Console.WriteLine(ex);
            }
        }
    }
}
