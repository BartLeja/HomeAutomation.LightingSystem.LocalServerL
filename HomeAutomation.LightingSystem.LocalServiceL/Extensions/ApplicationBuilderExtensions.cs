using System.Threading.Tasks;
using HomeAutomation.LightingSystem.LocalServiceL.Clients;
using HomeAutomation.LightingSystem.LocalServiceL.Models;
using HomeAutomation.LightingSystem.LocalServiceL.Mqtt;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace HomeAutomation.LightingSystem.LocalServiceL.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task RunSignalRClientAsync(
          this IApplicationBuilder applicationBuilder,
          IOptions<Configuration> configuration,
          ISignalRClient signalRClient,
          IRestClient restClient)
        {
            var auth = configuration.Value.AuthorizationCredentials;

            var token = await restClient.GetToken(configuration.Value.IdentityServerBaseUrl, auth);
            await signalRClient.ConnectToSignalR(token, configuration.Value.SignalRHubUrl);
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
