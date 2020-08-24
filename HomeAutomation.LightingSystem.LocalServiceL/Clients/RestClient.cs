using Flurl;
using Flurl.Http;
using HomeAutomation.LightingSystem.LocalServiceL.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace HomeAutomation.LightingSystem.LocalServiceL.Clients
{
    public class RestClient: IRestClient
    {
        private readonly string _homeAutomationLightSystemApi;
        private readonly string _homeAutomationLocalLightSystemId;
        private readonly string _identityServiceApi;
     
        private readonly string lightPointUrl = "LightPoint";
        private readonly IConfiguration _configuration;

        public RestClient(
            //string homeAutomationLightSystemApi
            IConfiguration configuration
            )
        {
            _configuration = configuration;
          
            _homeAutomationLightSystemApi = _configuration.GetSection("HomeAutomationLightSystemApi").Value;
            _identityServiceApi = _configuration.GetSection("IdentityServiceUrl").Value;
            _homeAutomationLocalLightSystemId = _configuration.GetSection("HomeAutomationLocalLightingSystemId").Value;
        }

        public async Task<string> GetToken()
        {
            var respone = await $"{_identityServiceApi}/api/Authentication"
                .WithHeaders(new { X_Api_Key = "C5BFF7F0-B4DF-475E-A331-F737424F013C",
                    Home_Automation_Local_LightSystem_Id  = _homeAutomationLocalLightSystemId
                })
                .GetAsync();

            var contentInJson = respone.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TokenDto>(contentInJson).Token;
        }

        public async Task AddLightPoint(Guid homeLightSystemId, LightPointDto lightPointDto)
        {
            try
            {
                var token = await GetToken();
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/{homeLightSystemId}"
                    .SetQueryParams(new {
                      access_token = token
                    })
                    .PostJsonAsync(lightPointDto);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                //  throw;
            }
        }

        public async Task DisableLightPoint(Guid lightPointId)
        {
            try
            {
                var token = await GetToken();
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/disableLightPoint/{lightPointId}"
                     .SetQueryParams(new
                     {
                         access_token = token
                     })
                    .PostAsync(null);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        public async Task EnableLightPoint(Guid lightPointId)
        {
            try
            {
                var token = await GetToken();
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/enableLightPoint/{lightPointId}"
                     .SetQueryParams(new
                     {
                         access_token = token
                     })
                     .PostAsync(null);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        public async Task DeleteLightPoint(Guid lightPointId)
        {
            try
            {
                var token = await GetToken();
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/{lightPointId}"
                     .SetQueryParams(new
                     {
                         access_token = token
                     })
                     .DeleteAsync();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        public async Task DisableAllLightPoints(Guid homeLightSystemId)
        {
            try
            {
                var token = await GetToken();
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/disableAllLightPoints/{homeLightSystemId}"
                     .SetQueryParams(new
                     {
                         access_token = token
                     })
                     .PostAsync(null);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        public async Task EnableAllLightPoints(Guid homeLightSystemId)
        {
            try
            {
                var token = await GetToken();
                await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/enableAllLightPoints/{homeLightSystemId}"
                     .SetQueryParams(new
                     {
                         access_token = token
                     })
                     .PostAsync(null);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        public async Task<LightPointDto> GetLightPoints(Guid Id)
        {
            try
            {
                var token = await GetToken();
                return await $"{_homeAutomationLightSystemApi}/{lightPointUrl}/{Id}"
                     .SetQueryParams(new
                     {
                         access_token = token
                     })
                     .GetJsonAsync<LightPointDto>();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }
    }
}
