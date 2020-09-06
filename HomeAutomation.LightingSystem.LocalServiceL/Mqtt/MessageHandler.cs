using HomeAutomation.LightingSystem.LocalServiceL.Clients;
using HomeAutomation.LightingSystem.LocalServiceL.Dto;
using HomeAutomation.LightingSystem.LocalServiceL.Enums;
using HomeAutomation.LightingSystem.LocalServiceL.Handlers.SwitchLightPointEvent;
using HomeAutomation.LightingSystem.LocalServiceL.Models;
using MediatR;
using MQTTnet;
using MQTTnet.Client.Receiving;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Mqtt
{
    public class MessageHandler : IMqttApplicationMessageReceivedHandler
    {
        private readonly IRestClient _restClient;
        private readonly IMediator _mediator;
        private readonly ISignalRClient _signalRClient;

        public MessageHandler(IRestClient restClient, IMediator mediator,  ISignalRClient signalRClient)
        {
            _restClient = restClient;
            _mediator = mediator;
            _signalRClient = signalRClient;
        }

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            string payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
            string topic = eventArgs.ApplicationMessage.Topic;

            if (topic.Equals(MqttMessagesType.ConnectedToServer.ToString()))
            {
                await UserConnected(payload);
            }
            else if (topic.Equals(MqttMessagesType.SwitchLightChange.ToString()))
            {
                await SwitchLight(payload);
            }
            else if (topic.Equals(MqttMessagesType.LightPointReset.ToString()))
            {
                await LightPointReset(payload);
            }
        }

        private async Task UserConnected(string payload)
        {
            //TODO try catch serializer can crash
            LightPoint lightPointSwitch = JsonConvert.DeserializeObject<LightPoint>(payload);
            try
            {
                var lightPoint = await _restClient.GetLightPoints(Guid.Parse(lightPointSwitch.Id));
                if (lightPoint.CustomName == null)
                {
                    var lightBulb = new List<LightBulbDto>();
                    foreach (var id in lightPointSwitch.BulbsId)
                    {
                        lightBulb.Add(new LightBulbDto() { Id = Guid.Parse(id) });
                    }

                    var lightPointDto = new LightPointDto()
                    {
                        CustomName = lightPointSwitch.CustomName,
                        Id = Guid.Parse(lightPointSwitch.Id),
                        LightBulbs = lightBulb
                    };

                    //TODO auto generation of guid and saving to memeory of rPi
                    await _restClient.AddLightPoint(Guid.Parse("bdcd95ec-2dc6-4a7b-90ca-f132a7784b0f"), lightPointDto);
                }
                else
                {
                    await _restClient.EnableLightPoint(Guid.Parse(lightPointSwitch.Id));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task SwitchLight(string payload)
        {
            var lightPointStatus = JsonConvert.DeserializeObject<LightPointStatus>(payload);
             await _signalRClient.InvokeSendStatusMethod(Guid.Parse(lightPointStatus.Id), lightPointStatus.Status);
           // await _mediator.Send(new SwitchLightPointEvent(Guid.Parse(lightPointStatus.Id), lightPointStatus.Status));
        }

        private async Task LightPointReset(string payload)
        {
            LightPointRest lightPointReset = JsonConvert.DeserializeObject<LightPointRest>(payload);
            await _restClient.DeleteLightPoint(Guid.Parse(lightPointReset.Id));
        }
    }
}
