using System.Threading.Tasks;
using HomeAutomation.Core.Logger;
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
          ISignalRClient signalRClient
          )
        {
            // var auth = configuration.Value.AuthorizationCredentials;
            var signalRHubUrl = configuration.GetSection("SignalRHubUrl").Value;
            await signalRClient.ConnectToSignalR(signalRHubUrl);
        }

        public static async Task RunMqttServerAsync(
            this IApplicationBuilder applicationBuilder,
            IHomeAutomationMqttServer homeAutomationMqttServer, 
            IRestClient restClient,
            IMediator mediator,
            ISignalRClient signalRClient,
            IConfiguration configuration,
            ITelegramLogger telegramLogger,
            ILokiLogger lokiLogger)
        {
            var mqttServer = await homeAutomationMqttServer.ServerRun();
            mqttServer.ClientConnectedHandler = new ClientConnectedHandler(lokiLogger);
            mqttServer.ClientDisconnectedHandler = new ClientDisconnectedHandler(restClient, mqttServer, lokiLogger);
            mqttServer.ApplicationMessageReceivedHandler = new MessageHandler(
                restClient, 
                mediator,
                signalRClient, 
                configuration,
                telegramLogger,
                lokiLogger);
        }
    }
}
