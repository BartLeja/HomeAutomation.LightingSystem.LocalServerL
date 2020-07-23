using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAutomation.LightingSystem.LocalServiceL.Clients;
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
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
//https://dev.to/azure/net-core-iot-raspberry-pi-linux-and-azure-iot-hub-learn-how-to-build-deploy-and-debug-d1f
namespace HomeAutomation.LightingSystem.LocalServiceL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMediatR(typeof(Startup));
            services.AddSingleton<ISignalRClient, SignalRClient>();
            services.AddSingleton<IHomeAutomationMqttServer, Mqtt.MqttServer>();
            services.AddScoped<IRestClient, RestClient>();
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

            var auth = new Authorizationcredentials()
            {
                UserEmail = "blejaService",
                UserPassword = "test"
            };
            var token = await restClient.GetToken("https://homeautomationidentityservice20190519114609.azurewebsites.net", auth);
            await signalRClient.ConnectToSignalR(token, "https://lightingsystemapi20200320102759.azurewebsites.net/HomeLightSystemHub");
            var mqttServer = await homeAutomationMqttServer.ServerRun();
            mqttServer.ClientConnectedHandler = new ClientConnectedHandler();
            mqttServer.ClientDisconnectedHandler = new ClientDisconnectedHandler(restClient, mqttServer);
            mqttServer.ApplicationMessageReceivedHandler = new MessageHandler(restClient, mediator);
        }
    }
}
