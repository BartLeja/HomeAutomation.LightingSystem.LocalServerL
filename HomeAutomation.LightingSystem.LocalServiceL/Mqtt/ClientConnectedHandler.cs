using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Mqtt
{
    public class ClientConnectedHandler : IMqttServerClientConnectedHandler
    {
        public async Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            Debug.WriteLine($"Mqqt Client connected {eventArgs.ClientId}");
        }
    }
}
