namespace HomeAutomation.LightingSystem.LocalServiceL.Models
{
    public class Configuration
    {
        public string IdentityServerBaseUrl { get; set; }
        public string SignalRHubUrl { get; set; }
        public string HomeAutomationLightingSystemApi { get; set; }
        public string HomeAutomationLightingSystemId { get; set; }
        public AuthorizationCredentials AuthorizationCredentials { get; set; }
    }
}
