using Newtonsoft.Json;

namespace HomeAutomation.LightingSystem.LocalServiceL.Dto
{
    public class TokenDto
    {
        [JsonProperty("Token")]
        public string Token { get; set; }
    }
}
