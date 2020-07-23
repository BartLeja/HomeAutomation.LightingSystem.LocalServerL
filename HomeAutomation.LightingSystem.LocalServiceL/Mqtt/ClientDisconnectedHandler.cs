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

        public ClientDisconnectedHandler(IRestClient restClient, IMqttServer mqttServer)
        {
            _restClient = restClient;
            _mqttServer = mqttServer;
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
                Console.WriteLine(ex);
            }
        }
    }
}
