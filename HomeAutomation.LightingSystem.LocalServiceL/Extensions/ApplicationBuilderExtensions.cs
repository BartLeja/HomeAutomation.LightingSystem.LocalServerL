using System.Threading.Tasks;
using HomeAutomation.LightingSystem.LocalServiceL.Clients;
using HomeAutomation.LightingSystem.LocalServiceL.Models;
using HomeAutomation.LightingSystem.LocalServiceL.Mqtt;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HomeAutomation.LightingSystem.LocalServiceL.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task RunSignalRClientAsync(
          this IApplicationBuilder applicationBuilder,
          IConfiguration configuration,
          ISignalRClient signalRClient,
          IRestClient restClient)
        {
            // var auth = configuration.Value.AuthorizationCredentials;
            var signalRHubUrl = configuration.GetSection("SignalRHubUrl").Value;
            var token = await restClient.GetToken();
            await signalRClient.ConnectToSignalR(token, signalRHubUrl);
        }

        public static async Task RunMqttServerAsync(
            this IApplicationBuilder applicationBuilder,
            IHomeAutomationMqttServer homeAutomationMqttServer, 
            IRestClient restClient,
            IMediator mediator)
        {
            var mqttServer = await homeAutomationMqttServer.ServerRun();
            mqttServer.ClientConnectedHandler = new ClientConnectedHandler();
            mqttServer.ClientDisconnectedHandler = new ClientDisconnectedHandler(restClient, mqttServer);
            mqttServer.ApplicationMessageReceivedHandler = new MessageHandler(restClient, mediator);
        }
    }
}
