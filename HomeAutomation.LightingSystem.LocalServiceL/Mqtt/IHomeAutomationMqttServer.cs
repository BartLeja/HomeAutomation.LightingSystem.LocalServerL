using MQTTnet.Server;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Mqtt
{
    public interface IHomeAutomationMqttServer
    {
        Task<IMqttServer> ServerRun();
        Task PublishMessage(string topic, string payload);
    }
}
