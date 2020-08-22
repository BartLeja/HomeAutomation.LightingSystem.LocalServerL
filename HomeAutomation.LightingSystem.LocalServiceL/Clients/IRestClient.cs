using HomeAutomation.LightingSystem.LocalServiceL.Dto;
using HomeAutomation.LightingSystem.LocalServiceL.Models;
using System;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Clients
{
    public interface IRestClient
    {
        Task<string> GetToken();
        Task AddLightPoint(Guid homeLightSystemId, LightPointDto lightPointDto);
        Task DisableLightPoint(Guid lightPointId);
        Task EnableLightPoint(Guid lightPointId);
        Task DeleteLightPoint(Guid lightPointId);
        Task DisableAllLightPoints(Guid homeLightSystemId);
        Task EnableAllLightPoints(Guid homeLightSystemId);
        Task<LightPointDto> GetLightPoints(Guid Id);
    }
}
