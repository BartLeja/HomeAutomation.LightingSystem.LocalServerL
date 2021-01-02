using HomeAutomation.LightingSystem.LocalServiceL.Clients;
using MQTTnet;
using MQTTnet.Server;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Mqtt
{
    public class MqttServer : IHomeAutomationMqttServer
    {
        private IMqttServer mqttServer;
        private MqttServerOptionsBuilder optionsBuilder;
        //private IRestClient _restClient;

        public MqttServer(){}

        public async Task<IMqttServer> ServerRun()
        {
            Debug.WriteLine("TestMQTTServer");
            optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(8081);

            mqttServer = new MqttFactory().CreateMqttServer();

            await mqttServer.StartAsync(optionsBuilder.Build());

            //mqttServer.ClientConnectedHandler = new ClientConnectedHandler();
            //mqttServer.ClientDisconnectedHandler = new ClientDisconnectedHandler(_restClient, mqttServer); 
            // mqttServer.ApplicationMessageReceivedHandler = new MessageHandler(
            //     _settings, 
            //     _restClient, 
            //     _signalRClient);
        
            return mqttServer;
        }

        public async Task PublishMessage(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await mqttServer.PublishAsync(message);
        }
    }
}
