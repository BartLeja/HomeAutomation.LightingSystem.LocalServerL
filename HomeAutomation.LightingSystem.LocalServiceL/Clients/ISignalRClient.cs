using System;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Clients
{
    public interface ISignalRClient
    {
        Task ConnectToSignalR(string token, string signalRHubUrl);
        Task InvokeSendStatusMethod(Guid lightBulbId, bool status);
        Task InvokeSendMessageMethod(string user, string message);
    }
}
