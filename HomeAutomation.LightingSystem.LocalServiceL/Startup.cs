using HomeAutomation.LightingSystem.LocalServiceL.Clients;
using HomeAutomation.LightingSystem.LocalServiceL.LogManagment;
using HomeAutomation.LightingSystem.LocalServiceL.Extensions;
using HomeAutomation.LightingSystem.LocalServiceL.Models;
using HomeAutomation.LightingSystem.LocalServiceL.Mqtt;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

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
            services.Configure<Configuration>(_configuration.GetSection("Configuration"));
            services.AddSingleton<ISignalRClient, SignalRClient>();
            services.AddSingleton<IHomeAutomationMqttServer, Mqtt.MqttServer>();
            services.AddScoped<IRestClient, RestClient>();
            services.AddSingleton<ILogger, LokiLogger>();
            //services.AddScoped<IMqttServerClientDisconnectedHandler, ClientDisconnectedHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            ISignalRClient signalRClient,
            IRestClient restClient,
            IHomeAutomationMqttServer homeAutomationMqttServer,
            IMediator mediator,
            IOptions<Configuration> config)
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

            await app.RunSignalRClientAsync(_configuration, signalRClient, restClient);

            await app.RunMqttServerAsync(
                homeAutomationMqttServer,
                restClient,
                mediator,
                signalRClient,
                _configuration);
        }
    }
}
