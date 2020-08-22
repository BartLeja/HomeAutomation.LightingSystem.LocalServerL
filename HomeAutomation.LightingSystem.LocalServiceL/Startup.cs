using HomeAutomation.LightingSystem.LocalServiceL.Clients;
using HomeAutomation.LightingSystem.LocalServiceL.LogManagment;
using HomeAutomation.LightingSystem.LocalServiceL.Models;
using HomeAutomation.LightingSystem.LocalServiceL.Mqtt;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Server;
using System;
//https://dev.to/azure/net-core-iot-raspberry-pi-linux-and-azure-iot-hub-learn-how-to-build-deploy-and-debug-d1f
namespace HomeAutomation.LightingSystem.LocalServiceL
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMediatR(typeof(Startup));
            services.AddSingleton<ISignalRClient, SignalRClient>();
            services.AddSingleton<IHomeAutomationMqttServer, Mqtt.MqttServer>();
            services.AddScoped<IRestClient, RestClient>();
            services.AddSingleton<ILogger, LokiLogger>();
            //services.AddScoped<IMqttServerClientDisconnectedHandler, ClientDisconnectedHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            ISignalRClient signalRClient,
            IRestClient restClient,
            IHomeAutomationMqttServer homeAutomationMqttServer,
            IMediator mediator)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //var auth = new Authorizationcredentials()
            //{
            //    UserEmail = "blejaService",
            //    UserPassword = "test"
            //};
           // var identityServiceUrl = _configuration.GetSection("IdentityServiceUrl").Value;
            var signalRHubUrl = _configuration.GetSection("SignalRHubUrl").Value;
            var token = await restClient.GetToken();
           // var test = await restClient.GetLightPoints(Guid.Parse("49d8bb19-62a8-4496-88e6-8b010038d026"));
            await signalRClient.ConnectToSignalR(token, signalRHubUrl);
            var mqttServer = await homeAutomationMqttServer.ServerRun();
            mqttServer.ClientConnectedHandler = new ClientConnectedHandler();
            mqttServer.ClientDisconnectedHandler = new ClientDisconnectedHandler(restClient, mqttServer);
            mqttServer.ApplicationMessageReceivedHandler = new MessageHandler(restClient, mediator);
        }
    }
}
