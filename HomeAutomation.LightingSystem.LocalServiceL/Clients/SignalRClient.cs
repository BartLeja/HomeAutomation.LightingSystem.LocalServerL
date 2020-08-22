
using HomeAutomation.LightingSystem.LocalServiceL.Handlers.LightPointHardResetEvent;
using HomeAutomation.LightingSystem.LocalServiceL.Handlers.ReceiveLightPointEvent;
using HomeAutomation.LightingSystem.LocalServiceL.LogManagment;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Clients
{
    public class SignalRClient: ISignalRClient
    {
        private HubConnection _connection;
        private readonly IMediator _mediator;
        private ILogger _logger;

        public SignalRClient(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task ConnectToSignalR(string token, string signalRHubUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(signalRHubUrl,
                options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .Build();

            _connection.Closed += async (error) =>
            {
                var connectionState = false;
                while (!connectionState)
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    try
                    {
                        await _connection.StartAsync();
                        connectionState = true;
                    }
                    catch (Exception ex)
                    {
                        connectionState = false;
                        Console.WriteLine(ex);
                    }
                }
            };

            _connection.On<string, string>("ReceiveMessage", async (user, message) =>
            {
                Debug.WriteLine($"Message from {user} recived. {message}");
             
                // _logger.Log($"Message from {user} recived. {message}", typeof(SignalRClient).Namespace, "");
            });

            _connection.On<Guid, bool>("ReceiveLightPointStatus", (Guid lightBulbId, bool status) =>
            {
                Debug.WriteLine($"Message from {lightBulbId} recived.");
                _logger.LogInformation($"Message from {lightBulbId} recived.");
                _mediator.Send(new ReceiveLightPointEvent(lightBulbId, status));
            });

            _connection.On<Guid>("HardRestOfLightPoint", (Guid lightBulbId) =>
            {
                _mediator.Send(new LightPointHardResetEvent(lightBulbId));
            });


            _connection.On<Guid>("RestOfLightPoint", (Guid lightBulbId) =>
            {
                _mediator.Send(new LightPointHardResetEvent(lightBulbId));
            });

            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
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
                Console.WriteLine(ex);
            }
        }

        public async Task InvokeSendMessageMethod(string user, string message)
        {
            Debug.WriteLine("SendMessage");
            //_logger.Log("SendMessage", typeof(SignalRClient).Namespace, "");
            try
            {
                await _connection.InvokeAsync("SendMessage", user, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
