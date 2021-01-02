
using HomeAutomation.Core.Logger;
using HomeAutomation.LightingSystem.LocalServiceL.Handlers.LightPointHardResetEvent;
using HomeAutomation.LightingSystem.LocalServiceL.Handlers.ReceiveLightPointEvent;
using HomeAutomation.LightingSystem.LocalServiceL.Handlers.ReceiveLightsPointGroupEvent;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Clients
{
    public class SignalRClient: ISignalRClient
    {
        private HubConnection _connection;
        private readonly IMediator _mediator;
        private ITelegramLogger _logger;
        private ILokiLogger _lokiLogger;
        private readonly IRestClient _restClient;

        public SignalRClient(
            IMediator mediator,
            ITelegramLogger logger,
            ILokiLogger lokiLogger,
            IRestClient restClient)
        {
            _mediator = mediator;
            _logger = logger;
            _lokiLogger = lokiLogger;
            _restClient = restClient;        
        }

        public async Task ConnectToSignalR(string signalRHubUrl)
        {
            var token = await _restClient.GetToken();
            _connection = BuildHubConnection(signalRHubUrl, token);

            _connection.Closed += async (error) =>
            {
                await _logger.SendMessage("Lighting System SignalR Disconnected");
                var connectionState = false;
                while (!connectionState)
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    try
                    {
                        var token = await _restClient.GetToken();
                        _connection = BuildHubConnection(signalRHubUrl, token);
                        SignalRMethodsRegistration();
                        await _connection.StartAsync();
                        connectionState = true;
                    }
                    catch (Exception ex)
                    {
                        await _lokiLogger.SendMessage($"Lighting System {ex}", LogLevel.Error);
                        connectionState = false;
                        Console.WriteLine(ex);
                    }
                }
            };

            SignalRMethodsRegistration();

            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                await _lokiLogger.SendMessage($"Lighting System {ex}", LogLevel.Error);
                //Here can be retry
                Console.WriteLine(ex);
            }

        }

        public async Task InvokeSendStatusMethod(Guid lightBulbId, bool status)
        {
            try
            {
                await _connection.InvokeAsync("SendLightPointStatus", lightBulbId, status);
            }
            catch (Exception ex)
            {
                await _lokiLogger.SendMessage($"Lighting System {ex}", LogLevel.Error);
                Console.WriteLine(ex);
            }
        }

        public async Task InvokeSendMessageMethod(string user, string message)
        {
            Debug.WriteLine("SendMessage");
            try
            {
                await _connection.InvokeAsync("SendMessage", user, message);
            }
            catch (Exception ex)
            {
                await _lokiLogger.SendMessage($"Lighting System {ex}", LogLevel.Error);
                Console.WriteLine(ex);
            }
        }

        private HubConnection BuildHubConnection(string signalRHubUrl, string token)
        {
            return new HubConnectionBuilder()
                .WithUrl(signalRHubUrl,
                options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                //Try with that
               // .WithAutomaticReconnect()
                .Build();
        }

        private void SignalRMethodsRegistration()
        {
            _connection.On<string, string>("ReceiveMessage", async (user, message) =>
            {
                Debug.WriteLine($"Message from {user} revived. {message}");

                // _logger.Log($"Message from {user} recived. {message}", typeof(SignalRClient).Namespace, "");
            });

            _connection.On<Guid, bool>("ReceiveLightPointStatus", (lightBulbId, status) =>
            {
                _lokiLogger.SendMessage($"Lighting System Changed bulb {lightBulbId} status to {status}.");
                _mediator.Send(new ReceiveLightPointEvent(lightBulbId, status));
            });

            _connection.On<IEnumerable<Guid>, bool>("ReceiveLightPointsGroupStatus", (lightPointIds, status) =>
            {
                _lokiLogger.SendMessage($"Lighting System Changed group status to {status}");
                _mediator.Send(new ReceiveLightsPointGroupEvent(lightPointIds, status));
            });

            _connection.On<Guid>("HardRestOfLightPoint", (lightBulbId) =>
            {
                _lokiLogger.SendMessage($"Lighting System Hard reset of bulb {lightBulbId}");
                _mediator.Send(new LightPointHardResetEvent(lightBulbId));
            });

            _connection.On<Guid>("RestOfLightPoint", (lightBulbId) =>
            {
                _lokiLogger.SendMessage($"Lighting System Reset of bulb {lightBulbId}");
                _mediator.Send(new LightPointHardResetEvent(lightBulbId));
            });

        }
    }
}
